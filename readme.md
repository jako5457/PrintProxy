# PrintProxy
A system for monitoring and maintaining 3D printers

## Features
- Printer Connections
    - [X] Flashforge printers (New API)
    - [X] Octoprint
- Sevice Roles
    - Printer Controller
    - Printer Hub

- Upcoming features
    - Print UI (Hub)
        - Print Queue
        - Printer Grouping
        - Printer Reservation

## Controller 
   Controls a printers state and commands

   ### Environment Variables
   #### OctoPrint
   ```environment
   PRINTER_TYPE = octoprint
   OCTOPRINT_ENDPOINT = http://127.0.0.1:8080/ # Octoprint endpoint here
   OCTOPRINT_APIKEY = "123456" # API key here
   ```

   #### Flashforge
   ```environment
   PRINTER_TYPE = flashforge
   FLASHFORGE_IP = 127.0.0.1 # Ip or hostname of printer
   FLASHFORGE_SERIALNUMBER = 123456 # Serial number of printer
   FLASHFORGE_CODE = abc123 # CheckCode

   FLASHFORGE_PORT = 8898 # Optional uses 8898 by default
   ```