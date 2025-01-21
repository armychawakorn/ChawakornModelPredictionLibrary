# ChawakornModelPredictionLibrary

**ไลบรารีสำหรับเรียกใช้โมเดล Machine Learning ผ่าน Python จาก C#**

**สวัสดีครับ** ผม ชวกร เนืองภา นักศึกษาปริญญาตรี สาขาวิทยาการคอมพิวเตอร์และสารสนเทศ มหาวิทยาลัยขอนแก่นครับ ผมได้สร้างไลบรารีนี้ขึ้นมาเพื่อเป็นเครื่องมือที่ช่วยให้โปรแกรม C# สามารถเรียกใช้โมเดล Machine Learning ที่เขียนด้วย Python ได้อย่างง่ายดายและมีประสิทธิภาพ

## ภาพรวม

`ChawakornModelPredictionLibrary` เป็น Class Library สำหรับ .NET ที่ช่วยให้คุณสามารถเรียกใช้ Python script และรับผลลัพธ์จากการทำนายของโมเดล Machine Learning ที่เขียนด้วย Python ได้อย่างง่ายดาย ไลบรารีนี้เหมาะสำหรับโปรเจ็กต์ที่ต้องการรวมความสามารถด้าน Machine Learning ที่เขียนด้วย Python เข้ากับโปรแกรม C# ของคุณ

## คุณสมบัติหลัก

*   **การเรียกใช้ Python Script ที่ยืดหยุ่น:** รองรับการกำหนด path ของ Python executable และ Python script ได้อย่างอิสระ
*   **การส่ง Input ผ่าน Stream:** สามารถส่ง input ไปยัง Python script ผ่าน Standard Input
*   **การรับ Output ผ่าน Stream:** สามารถรับ output จาก Python script ผ่าน Standard Output
*   **การจัดการ Error อย่างมืออาชีพ:** มีการตรวจสอบ path และ handle exception อย่างเหมาะสม พร้อมทั้งส่ง error message ที่เข้าใจได้
*   **การทำงานแบบ Asynchronous:** รองรับการทำงานแบบ Asynchronous เพื่อให้โปรแกรมของคุณทำงานได้อย่างราบรื่นและไม่ค้าง
*   **ความปลอดภัย:** มีการตรวจสอบไฟล์และจัดการ Process อย่างปลอดภัย

## วิธีการใช้งาน

1.  **ติดตั้ง Library:**
    *   ดาวน์โหลดและเพิ่ม `ChawakornModelPredictionLibrary.dll` ลงในโปรเจ็กต์ C# ของคุณ
2.  **สร้าง Instance:** สร้าง instance ของ `PythonModelPrediction` โดยระบุ path ของ Python executable และ Python script:

    ```csharp
    using ChawakornModelPredictionLibrary;
    ...
    string pythonExecutablePath = @"C:\Python39\python.exe"; // แก้ไข path
    string pythonScriptPath = @"C:\MyProject\predict.py"; // แก้ไข path
    PythonModelPrediction predictor = new PythonModelPrediction(pythonExecutablePath, pythonScriptPath);
    ```

3.  **เรียกใช้ `StreamAsync` method:** ส่ง input ไปยัง Python script และรับผลลัพธ์โดยใช้ callback delegate:

    ```csharp
    string inputData = "6,148,72,35,33.6,0.627,50"; // Input data

    await predictor.StreamAsync(inputData, (int result) => {
        Console.WriteLine($"Prediction Result: {result}");
        if(result == 1) {
          //ทำการประมวลผลผลลัพธ์
        } else if (result == -1) {
           //จัดการ error ที่เกิดจาก python process
        } else if (result == -2){
          //จัดการ error ที่เกิดจาก output format ของ python
        }
        else if (result == -3){
           //จัดการ error ทั่วไปที่เกิดขึ้นใน python process
        }
    });

    ```
    `inputData` คือข้อมูลที่คุณต้องการส่งไปยัง python script
    delegate จะส่งค่าเป็น int โดยมี code `-1` แทน error ของ python process, `-2` แทน error จากรูปแบบ output ของ python, `-3` แทน error ทั่วไปที่เกิดขึ้น และ `1` แทนผลลัพธ์จากการทำนาย

## ข้อควรระวัง

*   ตรวจสอบให้แน่ใจว่า path ของ Python executable และ Python script นั้นถูกต้อง
*   ตรวจสอบให้แน่ใจว่า Python script ของคุณสามารถทำงานได้และส่ง output เป็นตัวเลขได้

## ตัวอย่าง Python Script (`predict.py`)

```python
import sys
import pandas as pd
from train.Utils_ModelManager import Utils

# Load the model
model = Utils.load_model('models/DecisionTree.pkl')

# Predict
x_input = sys.stdin.read().strip()
x_array = [float(x) for x in x_input.split(',')]
x = pd.DataFrame([x_array], columns=['Pregnancies', 'Glucose', 'BloodPressure', 'Insulin' ,'BMI', 'DiabetesPedigreeFunction', 'Age'])
y_pred = model.predict(x)
print(y_pred[0])
```

## ติดต่อ

หากมีข้อสงสัย หรือต้องการความช่วยเหลือเพิ่มเติม สามารถติดต่อผมได้ตามช่องทางด้านล่างนี้ครับ:

*   **อีเมล:** [Chawakorn.n@kkumail.com](mailto:Chawakorn.n@kkumail.com)
*   **โทรศัพท์มือถือ:** +66842979685
*   **Line:** [solid\_soul](https://line.me/ti/p/solid_soul)

ผมยินดีรับฟังทุกความคิดเห็นและข้อเสนอแนะเพื่อปรับปรุง Library นี้ให้ดียิ่งขึ้นครับ

**ขอขอบคุณที่ใช้ `ChawakornModelPredictionLibrary` ครับ!**
