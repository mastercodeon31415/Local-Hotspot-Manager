![PowerShell](https://img.shields.io/badge/PowerShell-5.1%2B-blue?logo=powershell)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blueviolet?logo=.net)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/mastercodeon31415/Local-Hotspot-Manager/blob/main/LICENSE)
[![GitHub issues](https://img.shields.io/github/issues/mastercodeon31415/Local-Hotspot-Manager)](https://github.com/mastercodeon31415/Local-Hotspot-Manager/issues)
[![GitHub stars](https://img.shields.io/github/stars/mastercodeon31415/Local-Hotspot-Manager)](https://github.com/mastercodeon31415/Local-Hotspot-Manager/stargazers)

Local Hotspot Manager is a user-friendly Windows Forms application for creating and managing a local Wi-Fi hotspot on your Windows machine. A standout feature of this tool is its ability to create a hotspot that functions without an active internet connection, making it perfect for setting up local networks for gaming, file sharing, or device-to-device communication. This is achieved by leveraging a loopback adapter, ensuring that devices can connect to your PC and communicate with each other on a local network.

![Main Application Window](https://github.com/user-attachments/assets/b85d683f-397d-4318-a2ad-f0e88fde8803)

## Features

*   **One-Time Setup:** The application handles all necessary system configurations with a single click.
*   **Create Local Hotspots:** Start a Wi-Fi hotspot without needing an internet connection.
*   **Customizable Hotspot:** Set your own Hotspot Name (SSID) and Password.
*   **Network Band Selection:** Choose between 5 GHz, 2.4 GHz, or Auto for your network band.
*   **Start on Login:** Configure the hotspot to start automatically when you log in to Windows.
*   **Simple Interface:** An intuitive and clean user interface makes managing your hotspot effortless.
*   **Dark Mode:** The application automatically adapts to the system's dark mode settings for a comfortable viewing experience.

## How It Works

The Local Hotspot Manager uses a combination of PowerShell scripts and Windows APIs to provide its functionality. Here's a breakdown of the key technical aspects:

*   **Loopback Adapter:** To create a hotspot without an internet connection, the application ensures a `Microsoft KM-TEST Loopback Adapter` is installed on your system.  This virtual network adapter is named 'Loopback' and is used to create a local network that devices can connect to. The application automates the installation and configuration of this adapter during the one-time setup process.

*   **Power Management:** Windows has power-saving features that can automatically turn off the mobile hotspot if no devices are connected. To ensure the hotspot remains active, the application makes two important changes during setup:
    1.  It disables the idle timeout for the Mobile Hotspot Service (icssvc) by setting the `PeerlessTimeoutEnabled` registry value to `0`.
    2.  It disables power management on your physical Wi-Fi adapter to prevent Windows from turning it off to save power.

*   **Hotspot Control:** The application uses the `Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager` class from the Windows Runtime (WinRT) to control the mobile hotspot. This allows it to programmatically start, stop, and configure the hotspot with the SSID, password, and network band you provide.

*   **Automatic Startup:** The "Start Hotspot automatically on Login" feature is implemented by creating a Scheduled Task in Windows. This task is configured to run when you log in and executes a PowerShell command to start the hotspot.

## Prerequisites

*   .NET Framework 4.7.2
*   Windows 10 or later
*   Administrative privileges (for the initial setup and for starting/stopping the hotspot)

## How to Use

1.  **Initial Setup**

    *   When you first launch the application, it will check if your system is configured correctly.
    *   If any prerequisites are missing (like the Loopback Adapter or power settings), you'll see the "System Prerequisites" screen.
    *   Click the **Perform One-Time Setup** button. The application will run the necessary PowerShell scripts to configure your system. This requires administrative privileges, so you may see a UAC prompt.
    *   Once the setup is complete, the main control panel will become available.

    ![System Prerequisites Check](https://github.com/user-attachments/assets/a2acbbc6-c4a6-4ca8-a151-69c6cd1e491b)

2.  **Configuring and Starting the Hotspot**

    *   In the "Hotspot Controls" section, you can set the following:
        *   **Hotspot Name (SSID):** The name of your Wi-Fi network.
        *   **Password:** The password for your network (must be at least 8 characters).
        *   **Network Band:** Choose between 5 GHz, 2.4 GHz, or Auto.
    *   Click the **Start Hotspot** button to begin broadcasting your local network. The status bar at the bottom will indicate that the hotspot is "ACTIVE".

3.  **Stopping the Hotspot**

    *   To turn off the hotspot, simply click the **Stop Hotspot** button. The status will change to "INACTIVE".

4.  **Enabling Auto-Start**

    *   If you want the hotspot to start automatically every time you log in to Windows, check the **Start Hotspot automatically on Login** box.
    *   The application will create a scheduled task to handle this. You can disable this at any time by unchecking the box.

![Main Controls](https://github.com/user-attachments/assets/b85d683f-397d-4318-a2ad-f0e88fde8803)

## Contributing

Contributions are welcome! If you have suggestions for improvements or encounter any issues, please feel free to open an issue or submit a pull request.

## Donation links

Anything is super helpful! Anything donated helps me keep developing this program and others!
- https://www.paypal.com/paypalme/lifeline42
- https://cash.app/$codoen314

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/mastercodeon31415/Local-Hotspot-Manager/blob/main/LICENSE) file for details. 