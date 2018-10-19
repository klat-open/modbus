


# Modbus library  
  
To communicate with slaves, you must create an **ICommStream** (currently, this library supports for **SerialStream** via serial port). After that, call the request function corresponds witch each function code. The request function will send a request to slave and waiting for the response, at that time, other requests will be pending until the current one is completed.  
  
```csharp  
var serialPort = new SerialPort("COM1");
var stream = new SerialStream(serialPort);  
var responseBytes = stream.RequestFunc3(slaveId, dataAddress, registerCount);  
```  
## Supported function codes  (example and code)

### Read Holding Registers (FC=03)  
  
**Request**  
  
This command is requesting the content of analog output holding registers # 40108 to  
 40110 from the slave device with address 17.  
  
> 11 03 006B 0003 7687  
  
- 11: The Slave Address (11 hex = address17 )  
- 03: The Function Code 3 (read Analog Output Holding Registers)  
- 006B: The Data Address of the first register requested. ( 006B hex = 107 , + 40001 offset = input #40108 )  
- 0003: The total number of registers requested. (read 3 registers 40108 to 40110)    
- 7687: The CRC (cyclic redundancy check) for error checking.  
  
**Response**  
  
> 11 03 06 AE41 5652 4340 49AD  
  
- 11: The Slave Address (11 hex = address17 )  
- 03: The Function Code 3 (read Analog Output Holding Registers)  
- 06: The number of data bytes to follow (3 registers x 2 bytes each = 6 bytes)  
- AE41: The contents of register 40108  
- 5652: The contents of register 40109   
- 4340: The contents of register 40110  
- 49AD: The CRC (cyclic redundancy check).

**Code**
```csharp
// send request and waiting for response
// the request needs: slaveId, dataAddress, registerCount
var responseBytes = stream.RequestFunc3(0x11, 0x006B, 0x0003);
// extract the content part (the most important in the response)
var data= responseBytes.ToResponseFunc3().Data;
```

### Read Input Registers (FC=04)
**Request**

This command is requesting the content of analog input register # 30009  
from the slave device with address 17.

> 11 04 0008 0001 B298

- 11: The Slave Address (11  hex = address17 )  
- 04: The Function Code 4 (read Analog Input Registers)  
- 0008: The Data Address of the first register requested.  (  0008  hex = 8 , + 30001 offset = input register #30009 )  
- 0001: The total number of registers requested. (read 1 register)  
- B298: The CRC (cyclic redundancy check) for error checking.

**Response**

> 11 04 02 000A F8F4

- 11: The Slave Address (11  hex = address17 )  
- 04: The Function Code 4 (read Analog Input Registers)  
- 02: The number of data bytes to follow (1 registers x 2 bytes each = 2 bytes)  
- 000A: The contents of register 30009  
- F8F4: The CRC (cyclic redundancy check).

**Code**
```csharp
// send request and waiting for response
// the request needs: slaveId, dataAddress, registerCount
var responseBytes = stream.RequestFunc4(0x11, 0x0008, 0x0001);
// extract the content part (the most important in the response)
var data= responseBytes.ToResponseFunc4().Data;
```

### Preset Single Register (FC=06)

**Request**

This command is writing the contents of the analog output holding register # 40002  
to the slave device with address 17.

> 11 06 0001 0003 9A9B

- 11: The Slave Address (11  hex = address17 )  
- 06: The Function Code 6 (Preset Single Register)  
- 0001: The Data Address of the register. (  0001  hex = 1 , + 40001 offset = register #40002 )  
- 0003: The value to write  
- 9A9B: The CRC (cyclic redundancy check) for error checking.

**Response**

The normal response is an echo of the query, returned after the register contents have been written.

> 11 06 0001 0003 9A9B

- 11: The Slave Address (11  hex = address17 )  
- 06: The Function Code 6 (Preset Single Register)  
- 0001: The Data Address of the register. (# 40002 - 40001 = 1 )  
- 0003: The value written  
- 9A9B: The CRC (cyclic redundancy check) for error checking.

**Code**
```csharp
// send request and waiting for response
// the request needs: slaveId, dataAddress, writeValue
var responseBytes = stream.RequestFunc6(0x11, 0x0001, 0x0003);
// the reponse bytes should be an echo of the request.
```
### Diagnosis (FC=08)
The diagnosis function provides a series of tests for checking the communication system between the master and the slave and for examining a variety of internal error states within the slave.

**Request**

This command is diagnosing the slave id 0x0B with sub function is 0x0000 and data is 0x0203.

> 0B 08 0000 0203 A1C0

- 0B: The slave address.
- 08: The function code 8.
- 0000: The function uses two bytes in the query to specify a sub function code defining the test that is to be carried out. The slave returns the function code and the sub function code in the response.
- 0203: The diagnostics data or control information.
- A1C0: The CRC (cyclic redundancy check) for error checking.

**Response**

> 0B 08 0000 0203 A1C0

- 0B: The slave address.
- 08: The function code 8.
- 0000: The sub function that same likes the request.
- 0203: Echo data.
- A1C0: The CRC (cyclic redundancy check) for error checking.

**Code**

```csharp
// send request and waiting for response
// the request needs: slaveId, subFunction, data
var responseBytes = stream.RequestFunc8(0x0B, 0x0000, 0x0203);
```

### Preset Multiple Registers (FC=16)

**Request**

This command is writing the contents of two analog output holding registers # 40002 & 40003 to the slave device with address 17.

> 11 10 0001 0002 04 000A 0102 C6F0

- 11: The Slave Address (11  hex = address17 )  
- 10: The Function Code 16 (Preset Multiple Registers,  10  hex - 16 )  
- 0001: The Data Address of the first register. (  0001  hex = 1 , + 40001 offset = register #40002 )  
- 0002: The number of registers to write  
- 04: The number of data bytes to follow (2 registers x 2 bytes each = 4 bytes)  
- 000A: The value to write to register 40002  
- 0102: The value to write to register 40003  
- C6F0: The CRC (cyclic redundancy check) for error checking.

**Response**

> 11 10 0001 0002 1298

- 11: The Slave Address (17 =  11  hex)  
- 10: The Function Code 16 (Preset Multiple Registers,  10  hex - 16 )  
- 0001: The Data Address of the first register. (# 40002 - 40001 = 1 )  
- 0002: The number of registers written.  
- 1298: The CRC (cyclic redundancy check) for error checking.

**Code**
```csharp
// send request and waiting for response
// the request needs: slaveId, dataAddress, writeValue
var responseBytes = stream.RequestFunc16(0x11, 0x0001, new byte[] {0x00, 0x0A, 0x01, 0x02});
```

## Exception handling

There are several cases that the response is corrupted, nothing response (maybe the slave is down),... In these cases, an exception will be thrown, a `try catch` statement should be used to handle in these cases.

There are three kinds of exception:
- `EmptyResponsedException`: The slave does not respond anything, that seems like the slave is down, the connection is broken...
- `MissingDataException`: The response bytes is less than the required, ex: the required bytes length is 11 but the received bytes length is 9.
- `DataCorruptedException`: Checksum is failed, wrong response slave id, wrong response function code...

Example:
```csharp
try {
    var responseBytes = stream.RequestFunc3(0x11, 0x006B, 0x0003);
    // handle your response bytes
}
catch(Exception e) {
    if (e is DataCorruptedException) {
        BroadcastHandledExceptionEvent("checksum is failed", e);
    }
    else if (e is EmptyResponsedException) {
        BroadcastHandledExceptionEvent("request timeout", e);
    }
    else if (e is MissingDataException) {
        BroadcastHandledExceptionEvent("Missing response bytes", e);
    }
    else {
        throw e;
    }
}
```

## More options
The library contains several utilities that help you manipulate with response bytes more easier.

### Extract the response
Each function code has a parser that helps you extract the response to a structured data.

Example:
```csharp
var responseBytes = stream.RequestFunc3(0x11, 0x006B, 0x0003);
var response= responseBytes.ToResponseFunc3();
var data = response.Data;// the content part of the response
var slaveId = response.SlaveId;// it should be 0x11 in this case
```

### Manipulate bytes array
The **ByteUtils** class that contains several extension methods to convert from bytes to the number and vice versa.

Example1: the response data contains two values are heart rate (1 register - 2 bytes) and step count (2 register - 4 bytes).

- First 2 bytes: heart rate. -> bytes from 0 to 1
- Last 4 bytes: step count. -> bytes from 2 to 5

```csharp
var data= responseBytes.ToResponseFunc3().Data;
var heartRate = data.ToInt16(0);// 1 register contains heart rate value, bytes from 0 to 1
var stepCount = data.ToInt32(2);// 2 register contain step count value, bytes from 2 to 5
```

Example2: You want to use the function code 16 to write current time (Unix epoch) to the slave (of course, function 6 is easier in this case, but it's just an example)

- The slave id: 0x11
- The register address: 0x0001

```csharp
var unixTime = DateTime.Now.ToUnixEpoch();// convert current time to unix epoch
var responseBytes = stream.RequestFunc16(0x11, 0x0001, unixTime.ToBytes());
```