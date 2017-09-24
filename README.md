# C-Sharp-Asteroids-Game

### 問題
1. type_C的爆炸動畫串列檔案太大，需要再載入時先分割，再一一撥放，有待改良<br>
暫時用別張爆炸特效圖來代替
2. 關於Animation從Class換Struct就可正常使用的原因還有待確認

## ship-test1:<br>
基本移動與更新<br>
![Alt text](\example\test1.gif)

## ship-test2:<br>
雙緩衝<br>
FPS穩定<br>
![Alt text](\example\test2.gif)


## ship-test3:<br>
Animation函式庫<br>
Entity Class<br>
Bullet Class<br>
Ship Class<br>
![Alt text](\example\test3.gif)


## ship-test3.1:<br>
使用DeltaTime穩定移動速度


## ship-test4:<br>
加入行星<br>
當行星被子彈擊中時消失<br>
![Alt text](\example\test4.gif)


## ship-test5:<br>
加入爆炸特效<br>
修正Entity的Animation會同步撥放的問題，因為是以物件參考，所以全部都連到同個Animation<br>
解決方法:以將Class換成Struct<br>
![Alt text](\example\test5.gif)

