from selenium import webdriver
import chromedriver_binary             # パスを通すためのコード
import time
from selenium.webdriver.chrome.options import Options

import serialConnect

options = Options()
# 現在開いてるChromeにたいしてSeleniumを当てるためのオプション
options.add_experimental_option("debuggerAddress", "127.0.0.1:9222")
# 上記のオプションを付与したWebDriverの作成
driver = webdriver.Chrome(options=options)

# シリアル通信で受け取ったURLをChromeで開く
openUrl(serialConnect.serial_read)

"""
シリアル通信によって送られてきたURLをChromeで開く
@param string url
"""


def openUrl(url):
    driver.get(url)


"""
現在アクティブになっているChromeのURLのTabを取得する
@param null
@return string active_url
"""


def getCurrentUrl():
    active_url = driver.current_url
    print(active_url)



import serial
import time
import ChromeWindow

ser = serial.Serial('COM8', 9600, timeout=None)

"""
シリアル通信で送られてくるデータを取得する
@return line
"""


def serial_read():
    line = ser.readline()
    print(line)
    ser.close()
    return line


"""
シリアル通信を用いてデータを送信する
"""


def serial_send(url):
    time.sleep(10)
    url = url.encode()
    ser.write(url)

    ser.close()


serial_send(ChromeWindow.getCurrentUrl)

beContinue = True
while beContinue == True:
    recive_data = serial_read()
    if recive_data == 1:
        send_data = ChromeWindow.getCurrentUrl()
        serial_send(send_data)
    else:
        ChromeWindow.openUrl(recive_data)