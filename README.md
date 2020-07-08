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
        "assetSourceLink": "https://drtesthls.akamaized.net/hls/live/2014202/drtest/master-archive.m3u8?startTime=1594211393&endTime=1594211693&hdnts=exp=1594301693~acl=/hls/live/2014202/drtest/*~data=st=1594211393,et=1594211693,geo=dk~hmac=****************************************************************",
        "ipAvailability": "DK",
        "deliveryType": "Stream",
        "format": "HLS",
        "isTokenProtected": false,
        "isStreamLive": false
    },
    {
        "assetSourceLink": "https://drtesthls.akamaized.net/hls/live/2014202/drtest/master-archive.m3u8?startTime=1594211393&endTime=1594211693",
        "ipAvailability": "EU",
        "deliveryType": "Stream",
        "format": "HLS",
        "isTokenProtected": true,
        "tokenAcl": "/hls/live/2014202/drtest/*",
        "tokenPayload": "st=1594211393,et=1594211693,geo=eu",
        "isStreamLive": false,
        "tokenKeyId": "stream2od-test",
        "tokenQueryName": "hdnts",
        "tokenVersion": "edge-auth-1"
    }
]
```
