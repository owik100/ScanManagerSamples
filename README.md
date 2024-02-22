# Scan Manager Pro - Advanced configuration
[Scan Manager Pro](https://play.google.com/store/apps/details?id=com.lemonowik.scanmanagerpro) <img src="https://play-lh.googleusercontent.com/oWk4n17E8-HIAyxWvDuA6OKGLD5SV_vGVgE0Dnl6GbGf0qDXh9dkO_Yod9qoSrTtQwE=w240-h480-rw" width="128" height="128">  allows you to quickly scan barcodes, QR codes and others.
<br/>
Here you will learn how you can customize app behaviors with advanced settings.
<br/>
<br/>
The instructions provided are for version <b>1.0.x</b> of the application.

## Basics
Advanced settings allow you to write your own <b>C#</b> code to change behavior of application. We support <b>.NET8</b> and <b>C# 12</b>.
<br/>
For now, they are available events:
- OnScanDetected
- OnScanSaved
<br/>
Custom code will be stored as single XML file. So you can create this file in outside environment and load to your app by clicking "Import settings" <b>(1)</b>. (".xml or .txt file fromat is required). Later you can export settings by clicking "Export settings" <b>(2)</b>. Alternatively, you can write code directly in app by clicking "Edit code" <b>(3)</b>. The "Load Template" <b>(4)</b> option will load a template, which is a good starting point for code creation.
<br/>
<br/>

<p float="left">
<img src="https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/Img/AdvancedSettings.PNG" >
<img src="https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/Img/LoadCode.PNG" >
</p>

If there are any problems with the code, it is not compiled correctly or detected, they will be shown in the "Errors" <b>(5)</b> section.
<br/>
<br/>
Using checkboxes, we can choose whether we currently want our code to be executed while the application is running. <b>(6)</b>
The status column indicates whether the event is loaded and compiled correctly (✔️), there is a problem with it (❌) or it has not been loaded (❓) <b>(7)</b>.

<img src="https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/Img/AdvancedSettings error.PNG" >

## Creating code
Our file should have the following format:

``` xml
<ScanManagerTriggers>
        <Events>
           
        </Events>
 </ScanManagerTriggers>
```
Between the "Events" tag we can add tags corresponding to the **ScanDetected** and **ScanSaved** events:
``` xml
<ScanManagerTriggers>
        <Events>
            <ScanDetected>
               C# code
            </ScanDetected>

            <ScanSaved>
               C# code
            </ScanSaved>
         </Events>
 </ScanManagerTriggers>
```
And between these tags we can write code in C#

## Events
### OnScanDetected
This event is triggered when the application detects a barcode. We can, for example, decide whether we want to continue processing it or ignore it, based on e.g. the barcode format.
<br/>
<br/>
The C# code for this event should look like this template:
```csharp
 using ScanManagerTriggerReferences;
 namespace ScanManagerProTriggers
 {
    public class ScanDetectedTriggerClass {         
         public bool OnScanDetected(BarcodeResultTrigger inputValue)
         {
             return true;
         }
     }
 }
```

Required setps:
- Add using ScanManagerTriggerReferences;
- Namespace must be ScanManagerProTriggers
- Classname must be ScanDetectedTriggerClass
- Method name must be OnScanDetected, return a value of type bool, accept one parameter of type [BarcodeResultTrigger](https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/BarcodeResultTrigger.cs)

[BarcodeResultTrigger](https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/BarcodeResultTrigger.cs) represents the barcode that was detected. Based on its parameters, we can decide whether we want to continue processing this code or not. If the method returns false, we cancel the process, if it returns true, we continue.

#### Example - Detect only barcodes of Polish products, i.e. those that start with code 590
```csharp
 using ScanManagerTriggerReferences;
 namespace ScanManagerProTriggers
 {
    public class ScanDetectedTriggerClass {         
         public bool OnScanDetected(BarcodeResultTrigger inputValue)
         {
             if(inputValue.BarcodeType == BarcodeTypes.Product && inputValue.DisplayValue.StartsWith("590"))
             {
                 return true;
             }

             return false;
         }
     }
 }
```

### OnScanSaved
This event is triggered when the application saves a barcode. We can, for example, send a request to our API to perform some action.
<br/>
<br/>
The C# code for this event should look like this template:

```csharp
using ScanManagerTriggerReferences;
namespace ScanManagerProTriggers
{
   public class ScanSavedTriggerClass {         
        public void OnScanSavedTrigger(BarcodeResultTrigger inputValue)
        {
          
        }
    }
}
```
Required setps:
- Add using ScanManagerTriggerReferences;
- Namespace must be ScanManagerProTriggers
- Classname must be ScanSavedTriggerClass
- Method name must be OnScanSavedTrigger, return a void, accept one parameter of type [BarcodeResultTrigger](https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/BarcodeResultTrigger.cs)

  [BarcodeResultTrigger](https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/BarcodeResultTrigger.cs) represents the barcode that was saved. 

#### Example - Send a request to the API when a barcode has been scanned
```csharp
using System.Net.Http;
using ScanManagerTriggerReferences;
using System.Threading.Tasks;
namespace ScanManagerProTriggers
{
    public class ScanSavedTriggerClass
    {
        public void OnScanSavedTrigger(BarcodeResultTrigger inputValue)
        {
            Task.Run(async () =>
            {
                try
                {
                    //It is not recommended to use httpClient in this way, but for now we do not have the option to use a static variable or IHttpClientFactory. I will try to solve this problem in future versions.
                    using (var client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.107:8082" + "/POSTTEST");
                        var content = new StringContent("\"" + inputValue + "\"", null, "application/json");

                        request.Content = content;
                        var response = await client.SendAsync(request);
                    }
                }
                catch (System.Exception)
                {

                }
            });
        }
    }
}
```
##### Example in action (video) -  Scan codes and receive them in your own app
[API app example](https://github.com/owik100/ScanManagerSamples/tree/master/ScanManagerSamples/Minimal%20API%20Example)
<a href="https://www.youtube.com/shorts/M8o4v-CaSwc" target="_blank">
  <img src="https://img.youtube.com/vi/M8o4v-CaSwc/0.jpg" alt="Scan Maanger Pro API example" style="width:100%;">
  <p align="center"><kbd>▶️ Play video</kbd></p>
</a>

## Full example
```csharp
    <ScanManagerTriggers>
        <Events>
            <ScanDetected>
             using ScanManagerTriggerReferences;
             namespace ScanManagerProTriggers
             {
                public class ScanDetectedTriggerClass {         
                     public bool OnScanDetected(BarcodeResultTrigger inputValue)
                     {
                         if(inputValue.BarcodeType == BarcodeTypes.Product && inputValue.DisplayValue.StartsWith("590"))
                         {
                             return true;
                         }

                         return false;
                     }
                 }
             }
            </ScanDetected>

            <ScanSaved>
            using System.Net.Http;
            using ScanManagerTriggerReferences;
            using System.Threading.Tasks;
            namespace ScanManagerProTriggers
            {
                public class ScanSavedTriggerClass
                {
                    public void OnScanSavedTrigger(BarcodeResultTrigger inputValue)
                    {
                        Task.Run(async () =>
                        {
                            try
                            {
                                //It is not recommended to use httpClient in this way, but for now we do not have the option to use a static variable or IHttpClientFactory. I will try to solve this problem in future versions.
                                using (var client = new HttpClient())
                                {
                                    var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.107:8082" + "/POSTTEST");
                                    var content = new StringContent("\"" + inputValue + "\"", null, "application/json");

                                    request.Content = content;
                                    var response = await client.SendAsync(request);
                                }
                            }
                            catch (System.Exception)
                            {

                            }
                        });
                    }
                }
            }
            </ScanSaved>
         </Events>
 </ScanManagerTriggers>
```
## BarcodeResultTrigger implementation
[File](https://github.com/owik100/ScanManagerSamples/blob/master/ScanManagerSamples/ExternalFiles/BarcodeResultTrigger.cs)

```csharp
    public class BarcodeResultTrigger
    {
        public BarcodeTypes BarcodeType { get; set; }

        public BarcodeFormats BarcodeFormat { get; set; }

        public string DisplayValue { get; set; }

        public string RawValue { get; set; }

        public int ScanResultId { get; set; }
    }

    public enum BarcodeTypes
    {
        Unknown,
        ContactInfo,
        Email,
        Isbn,
        Phone,
        Product,
        Sms,
        Text,
        Url,
        WiFi,
        GeographicCoordinates,
        CalendarEvent,
        DriversLicense
    }

    [Flags]
    public enum BarcodeFormats
    {
        Code128 = 1,
        Code39 = 2,
        Code93 = 4,
        CodaBar = 8,
        DataMatrix = 0x10,
        Ean13 = 0x20,
        Ean8 = 0x40,
        Itf = 0x80,
        QRCode = 0x100,
        Upca = 0x200,
        Upce = 0x400,
        Pdf417 = 0x800,
        Aztec = 0x1000,
        All = 0xFFFF
    }
```
