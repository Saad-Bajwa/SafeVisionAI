import requests
import json
from datetime import datetime, UTC


API_BASE_URL = 'https://localhost:7184/api/Incidents/add'

payload = {
    "cameraId": 1,
    "type": "Fire",
    "description": "Smoke detected near entrance",
    "confidenceScore": 0.92,
    "severity": "Critical",
    "detectedAt": datetime.now(UTC).isoformat(),
    "videoClipUrl": "https://example.com/clip.mp4",
    "imageUrl": "https://example.com/frame.jpg",
    "boundingBox": {
        "x": 100,
        "y": 200,
        "width": 50,
        "height": 80
    },
    "notes": "Triggered from AI test stub"
}

# Disable SSL verification for localhost development
response = requests.post(API_BASE_URL, json=payload, verify=False)
print(f"Status code: {response.status_code}")
print("Response: ", json.dumps(response.json(), indent=2))