# PrintProxy
A system for monitoring and maintaining 3D printers

## Features
- Printer Connections
    - [X] Flashforge printers (New API)
    - [X] Octoprint

- Upcoming features
    - Print UI
        - Print Queue
        - Printer Grouping
        - Printer Reservation

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
    ]
}
```