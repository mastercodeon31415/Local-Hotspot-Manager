using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NativeDarkMode_Lib;
using NativeDarkMode_Lib.Utils;

namespace HotspotManager
{
    public partial class Form1 : Form
    {
        private readonly int sleepTime = 500;

        public Form1()
        {
            InitializeComponent();

            Converter.DarkModeEnable(this);
        }

        

        #region Prerequisite and Setup Logic

        private async Task PerformPrerequisiteChecks()
        {
            pnlSetup.Visible = true;
            grpMainControls.Enabled = false;
            rtbSetupLog.Clear();
            Log("Checking core system configuration...");

            var loopbackExists = await CheckScriptCondition("if (Get-NetAdapter -Name 'Loopback' -ErrorAction SilentlyContinue) { $true } else { $false }");
            Log($"[1/2] Loopback Adapter: {(loopbackExists ? "Found" : "MISSING")}", loopbackExists);

            var powerSettingCorrect = await CheckScriptCondition("if ((Get-ItemProperty -Path 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\icssvc\\Settings' -Name 'PeerlessTimeoutEnabled' -ErrorAction SilentlyContinue).PeerlessTimeoutEnabled -eq 0) { $true } else { $false }");
            Log($"[2/2] Power Saving Disabled: {(powerSettingCorrect ? "OK" : "MISSING")}", powerSettingCorrect);

            if (loopbackExists && powerSettingCorrect)
            {
                Log("\nCore prerequisites met!", true);
                pnlSetup.Visible = false;
                grpMainControls.Enabled = true;
                await UpdateStartupTaskStatus();
                await CheckHotspotStatus();
            }
            else
            {
                Log("\nOne or more setup steps are required.", false);
                btnPerformSetup.Enabled = true;
            }
        }

        private async void btnPerformSetup_Click(object sender, EventArgs e)
        {
            btnPerformSetup.Enabled = false;
            rtbSetupLog.Clear();
            Log("Starting one-time system setup...");

            string setupScript = @"
                Import-Module NetAdapter -ErrorAction SilentlyContinue

                # Step 1: Create Loopback Adapter if it doesn't exist
                if (-not (Get-NetAdapter -Name 'Loopback' -ErrorAction SilentlyContinue)) {
                    Write-Host 'Installing Loopback Adapter...'
                    pnputil /add-driver C:\Windows\inf\netloop.inf /install | Out-Null
                    Get-NetAdapter -InterfaceDescription 'Microsoft KM-TEST Loopback Adapter' | Rename-NetAdapter -NewName 'Loopback' -Confirm:$false
                    Write-Host '-> Loopback Adapter created.'
                } else {
                    Write-Host 'Loopback Adapter already exists.'
                }

                # Step 2: Configure Power Settings
                Write-Host 'Configuring power settings...'
                $regPath = 'HKLM:\SYSTEM\CurrentControlSet\Services\icssvc\Settings'
                if (-not (Test-Path $regPath)) { New-Item -Path $regPath -Force | Out-Null }
                Set-ItemProperty -Path $regPath -Name 'PeerlessTimeoutEnabled' -Value 0 -Type DWord -Force
                
                $wifiAdapter = Get-NetAdapter -Physical | Where-Object { $_.MediaType -eq 'Native 802.11' } | Select-Object -First 1
                if ($wifiAdapter) {
                    Disable-NetAdapterPowerManagement -Name $wifiAdapter.Name -Confirm:$false
                    Write-Host '-> Wireless adapter power management disabled.'
                }

                Write-Host 'Setup complete!'
            ";

            var result = await RunExternalPowerShell(setupScript);
            foreach (var line in result.Output) { Log(line, true); }

            await PerformPrerequisiteChecks();
        }

        private void Log(string message, bool? success = null)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            rtbSetupLog.SelectionStart = rtbSetupLog.TextLength;
            rtbSetupLog.SelectionLength = 0;
            Color textColor = success.HasValue ? (success.Value ? Color.DarkGreen : Color.DarkRed) : Color.Silver;
            rtbSetupLog.SelectionColor = textColor;
            rtbSetupLog.AppendText(message + Environment.NewLine);
            rtbSetupLog.SelectionColor = rtbSetupLog.ForeColor;
            rtbSetupLog.ScrollToCaret();
        }

        #endregion

        #region Startup Task Management

        private async Task UpdateStartupTaskStatus()
        {
            chkStartup.Enabled = false;
            lblStatus.Text = "Status: Checking startup task...";
            bool taskExists = await CheckScriptCondition("if (Get-ScheduledTask -TaskName 'StartMobileHotspotAtLogin' -ErrorAction SilentlyContinue) { $true } else { $false }");
            chkStartup.Checked = taskExists;
            chkStartup.Enabled = true;
        }

        private async void chkStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkStartup.Enabled) return;

            chkStartup.Enabled = false;
            if (chkStartup.Checked)
            {
                lblStatus.Text = "Status: Creating startup task...";
                string startLogicForTask = @"`$profile = [Windows.Networking.Connectivity.NetworkInformation,Windows.Networking.Connectivity,ContentType=WindowsRuntime]::GetConnectionProfiles() | where {{$_.profilename -eq ''Loopback''}}; `$tether = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]::CreateFromConnectionProfile(`$profile); `$startOp = `$tether.StartTetheringAsync(); while (`$startOp.Status -eq 'Started') {{ Start-Sleep -m 100 }};";

                string createScript = $@"
                    $action = New-ScheduledTaskAction -Execute 'powershell.exe' -Argument '-ExecutionPolicy Bypass -Command ""{startLogicForTask}""'
                    $trigger = New-ScheduledTaskTrigger -AtLogOn
                    $principal = New-ScheduledTaskPrincipal -UserId (Get-CimInstance -ClassName Win32_ComputerSystem).Username -LogonType Interactive -RunLevel Highest
                    Register-ScheduledTask -TaskName 'StartMobileHotspotAtLogin' -Action $action -Trigger $trigger -Principal $principal -Description 'Starts the mobile hotspot on login.' -Force
                ";
                await RunExternalPowerShell(createScript);
                lblStatus.Text = "Status: Startup task created.";
            }
            else
            {
                lblStatus.Text = "Status: Removing startup task...";
                string deleteScript = "Unregister-ScheduledTask -TaskName 'StartMobileHotspotAtLogin' -Confirm:$false -ErrorAction SilentlyContinue";
                await RunExternalPowerShell(deleteScript);
                lblStatus.Text = "Status: Startup task removed.";
            }
            chkStartup.Enabled = true;
        }

        #endregion

        // This NEW method shells out to a real powershell.exe process (good for getting true system state)
        private Task<(bool Success, string[] Output)> RunExternalPowerShell(string command)
        {
            return Task.Run(() =>
            {
                try
                {
                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-ExecutionPolicy Bypass -Command \"{command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };

                    using (var process = new Process { StartInfo = processStartInfo })
                    {
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        string errors = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        if (process.ExitCode == 0 && string.IsNullOrEmpty(errors))
                        {
                            return (true, output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
                        }
                        else
                        {
                            return (false, errors.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
                        }
                    }
                }
                catch (Exception ex)
                {
                    return (false, new[] { ex.Message });
                }
            });
        }

        #region Hotspot Control Logic

        private async Task CheckHotspotStatus()
        {
            UpdateUIForChecking();

            // FINAL CORRECTION: Use the instance property instead of the failing static method.
            string script = $@"
                $connectionProfile = [Windows.Networking.Connectivity.NetworkInformation,Windows.Networking.Connectivity,ContentType=WindowsRuntime]::GetConnectionProfiles() | where {{ $_.profilename -eq 'Loopback' }}
                if ($null -ne $connectionProfile) {{
                    $tetheringManager = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]::CreateFromConnectionProfile($connectionProfile)
                    return $tetheringManager.TetheringOperationalState.ToString()
                }} else {{
                    return 'Off' # If loopback doesn't exist, it must be off
                }}";
            var result = await RunExternalPowerShell(script);
            if (result.Success && result.Output.FirstOrDefault() == "On")
            {
                UpdateUIForActiveHotspot();
                await GetCurrentHotspotConfig();
            }
            else { UpdateUIForInactiveHotspot(); }
        }

        private async Task GetCurrentHotspotConfig()
        {
            string script = $@"
                $tetheringManager = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]
                $config = $tetheringManager::GetCurrentAccessPointConfiguration()
                return @{{Ssid=$config.Ssid; Band=$config.Band.ToString()}}";
            var result = await RunExternalPowerShell(script);
            if (result.Success && result.Output.Any())
            {
                var configData = result.Output.FirstOrDefault();
                if (configData != null)
                {
                    var ssidLine = configData.Split(';').FirstOrDefault(line => line.Contains("Ssid"));
                    var bandLine = configData.Split(';').FirstOrDefault(line => line.Contains("Band"));
                    if (ssidLine != null) txtSsid.Text = ssidLine.Split('=').LastOrDefault()?.Trim();
                    if (bandLine != null)
                    {
                        string bandValue = bandLine.Split('=').LastOrDefault()?.Trim();
                        if (bandValue == "FiveGigahertz") cmbBand.SelectedItem = "5 GHz";
                        else if (bandValue == "TwoPointFourGigahertz") cmbBand.SelectedItem = "2.4 GHz";
                        else cmbBand.SelectedItem = "Auto";
                    }
                }
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSsid.Text)) { MessageBox.Show("Hotspot Name (SSID) cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (txtPassword.Text.Length < 8) { MessageBox.Show("Password must be at least 8 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            UpdateUIForChecking();
            string band;
            switch (cmbBand.SelectedItem.ToString())
            {
                case "5 GHz": band = "[Windows.Networking.NetworkOperators.TetheringWiFiBand]::FiveGigahertz"; break;
                case "2.4 GHz": band = "[Windows.Networking.NetworkOperators.TetheringWiFiBand]::TwoPointFourGigahertz"; break;
                default: band = "[Windows.Networking.NetworkOperators.TetheringWiFiBand]::Auto"; break;
            }

            // FINAL CORRECTION: Removed .GetResults() and now check the status directly on the operation object.
            string script = $@"
                $connectionProfile = [Windows.Networking.Connectivity.NetworkInformation,Windows.Networking.Connectivity,ContentType=WindowsRuntime]::GetConnectionProfiles() | where {{ $_.profilename -eq 'Loopback' }}
                if ($null -eq $connectionProfile) {{
                    Write-Host 'Could not find the Loopback connection profile. Tethering cannot start.'
                    return
                }}
                $tetheringManager = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]::CreateFromConnectionProfile($connectionProfile)
                $config = New-Object Windows.Networking.NetworkOperators.NetworkOperatorTetheringAccessPointConfiguration
                $config.Ssid = '{txtSsid.Text}'
                $config.Passphrase = '{txtPassword.Text}'
                $config.Band = {band}
                
                $configureOp = $tetheringManager.ConfigureAccessPointAsync($config)
                while ($configureOp.Status -eq 'Started') {{ Start-Sleep -Milliseconds 100 }}

                $startOp = $tetheringManager.StartTetheringAsync()
                while ($startOp.Status -eq 'Started') {{ Start-Sleep -Milliseconds 100 }}

                if ($startOp.Status -ne 'Success') {{ Write-Host 'Failed to start hotspot. Status: $($startOp.Status)' }}";
            var result = await RunExternalPowerShell(script);
            if (!result.Success) { MessageBox.Show($"Failed to start hotspot:\n\n{string.Join("\n", result.Output)}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            await Task.Run(() => { System.Threading.Thread.Sleep(sleepTime); });
            await CheckHotspotStatus();
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            UpdateUIForChecking();

            // FINAL CORRECTION: Use the correct, static StopTetheringAsync method.
            string script = $@"
                $connectionProfile = [Windows.Networking.Connectivity.NetworkInformation,Windows.Networking.Connectivity,ContentType=WindowsRuntime]::GetConnectionProfiles() | where {{ $_.profilename -eq 'Loopback' }}
                if ($null -ne $connectionProfile) {{
                    $tetheringManager = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]::CreateFromConnectionProfile($connectionProfile)
                    $stopOp = $tetheringManager.StopTetheringAsync()
                    while ($stopOp.Status -eq 'Started') {{ Start-Sleep -Milliseconds 100 }}
                }}";
            await RunExternalPowerShell(script);
            await Task.Run(() => { System.Threading.Thread.Sleep(sleepTime); });
            await CheckHotspotStatus();
        }

        #endregion

        #region UI and PowerShell Helpers

        private void UpdateUIForActiveHotspot() { lblStatus.Text = "Status: Hotspot is ACTIVE"; lblStatus.BackColor = Color.LightGreen; btnStart.Enabled = false; btnStop.Enabled = true; txtSsid.Enabled = false; txtPassword.Enabled = false; }
        private void UpdateUIForInactiveHotspot() { lblStatus.Text = "Status: Hotspot is INACTIVE"; lblStatus.BackColor = Color.LightCoral; btnStart.Enabled = true; btnStop.Enabled = false; txtSsid.Enabled = true; txtPassword.Enabled = true; }
        private void UpdateUIForChecking() { lblStatus.Text = "Status: Checking..."; lblStatus.BackColor = SystemColors.Control; btnStart.Enabled = false; btnStop.Enabled = false; }
        private async Task<bool> CheckScriptCondition(string script) { var result = await RunExternalPowerShell(script); return result.Success && result.Output.FirstOrDefault()?.ToLower() == "true"; }
        #endregion

        private async void Form1_Load(object sender, EventArgs e)
        {
            cmbBand.SelectedItem = "5 GHz";
            await PerformPrerequisiteChecks();
        }
    }
}