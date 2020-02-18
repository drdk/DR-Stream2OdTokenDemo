# DR.Stream2OdTokenDemo

## How to configure Token

Go to the project dir and configure key 

```
dotnet user-secrets set key *******-your-key-***************
```

Example of VideoAssetLinks:
```json
[
    {
        "assetSourceLink": "https://dr01-lh.akamaihd.net/i/dk/dr01_0@147054/master.m3u8?start=1581686777&end=1581687077&hdnea=exp=1581773478~acl=/i/dk/dr01_0@147054/*~data=1581686777-1581687077~hmac=********************** token removed **************************",
        "ipAvailability": "DK",
        "deliveryType": "Stream",
        "format": "HLS",
        "isTokenProtected": false,
        "isStreamLive ": false
    },
    {
        "assetSourceLink": "https://dr01-lh.akamaihd.net/i/eu/dr01_0@147054/master.m3u8?start=1581686777&end=1581687077",
        "ipAvailability": "EU",
        "deliveryType": "Stream",
        "format": "HLS",
        "isTokenProtected": true,
        "tokenAcl": "/i/eu/dr01_0@147054/*",
        "tokenPayload": "1581686777-1581687077",
        "isStreamLive ": false
    }
]
```
