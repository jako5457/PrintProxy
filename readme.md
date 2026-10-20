# PrintProxy
A system for monitoring and maintaining 3D printers

> 🆒 This repository i suited to run in Coolify with the only setup is to edit the `PrinterConfig.json`

## Features

### Printer Connections

- [X] Flashforge printers (New API)
- [X] Octoprint
- [X] Moonraker
- [X] Bambu lab (LAN mode)
- [ ] Prusa Link

> More contributions of other printers are welcome

### Features
- [X] Printer Reservation
- [X] Send files and start a print on printer (Admin only for now) 

### Planned features in the future
- Print Queue
- Printer Grouping
- Http API for using in other applications
    - Direct upload from a slicer ( _Orcaslicer in mind_ )

### Config file `PrinterConfig.json`
```json
{
    "octoprint":[
        {
            "Endpoint":"<host address for octoprint>",
            "Api_key":"<Api key for octoprint>"
        }
    ],
    "flashforge":[
        {
            "Printer_ip":"<ip or hostname of flashforge printer>",
            "Port":8898,
            "Serialnumber":"<Serialnumber of printer>",
            "check_code":"<Check code of printer>"
        }
    ],
    "moonraker":[
        {
            "printer_name": "<printer name shown in UI>",
            "endpoint": "<host address for Moonraker>"
        }
    ],
    "bambu":[
        {
            "Printer_name": "<Name in ui>",
            "Printer_ip": "<Printer IP>",
            "Access_code": "<Printer Access code>",
            "Serial_number": "<The printer serial number>",
            "Developer_mode": false <- Set true if developer mode is enabled
        }
    ]
}
```