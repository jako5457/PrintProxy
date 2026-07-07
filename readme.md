# PrintProxy
A system for monitoring and maintaining 3D printers

> 🆒 This repository i suited to run in Coolify with the only setup is to edit the `PrinterConfig.json`

## Features

### Printer Connections

- [X] Flashforge printers (New API)
- [X] Octoprint
- [X] Moonraker
- [ ] Prusa Link
- [ ] Bambu lab (LAN mode)

> More contributions of other printers are welcome

### Planned features in the future
- Print Queue
- Printer Grouping
- Printer Reservation
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
    ]
}
```