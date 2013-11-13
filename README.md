DoyaChatServer
==============

## これは何？

[TechCrunch Tokyo 2013 Hackathon](http://jp.techcrunch.com/events/techcrunch-tokyo-2013/hackathon/) で作成した DoyaChat のサーバ側コードです。AppSocially 賞をいただきました。

* Windows Azure 上にクラウド サービスとしてデプロイすることを想定したプロジェクトです。
* クライアント側から GCM 用の Push ID をもらってユーザ登録とします。認証機能はありません。
* 任意のクライアントから、任意のクライアントへ ID ベースでメッセージを Push で送ることができます。
* また、顔の画像をアップロードしてもらったら、表情を [PUX 顔検出 API](http://www.panasonic.co.jp/pux/api_sdk/) で判定し、相手に今の大体の表情を特徴化して送ることができます。

#### チームメンバ
Kazuki, Takuya, Yuto, Takumi, Tatsuo, (Yusuke)

## 参照

* [クライアント側 Android 向けのコード](https://github.com/daisy1754/DoyaChat/)
* [発表スライド](https://www.slideshare.net/tatsuosakamoto/131112-tech-crunchtokyohackathonpresentationfinal)
* [メンバー たつおさんが書いてくれた Blog 記事](http://tatsuojapan.blogspot.jp/2013/11/techcrunch-tokyo-2013-hackathon-7.html)

## 必要なもの

* [Visual Studio 2013](http://www.microsoft.com/visualstudio/eng/downloads)
* [Windows Azure のアカウント](http://www.windowsazure.com/)
* [Google の新規 Cloud Project](https://cloud.google.com/console)
* [PUX の API 利用権](http://www.panasonic.co.jp/pux/api_sdk/)

## 動かすためには？

まず、[Web.config](https://github.com/yutopio/DoyaChatServer/blob/master/WFE/Web.config) に、GCM でのプッシュ通知に用いるためのサーバー API キー、顔認識のために使う PUX の API キーとベース アドレスを指定します。その後、Visual Studio 2013 でビルドし、Windows Azure にクラウド サービスを作成して、そちらにデプロイしてください。

[AWS](http://aws.amazon.com/) で動かしたい場合は、若干の変更が必要になります。具体的に [ChatController](https://github.com/yutopio/DoyaChatServer/blob/master/WFE/Controllers/ChatController.cs) 内で、GCM の登録キーとユーザ ID を関連付けているテーブルを、ファイル IO やデータベースなどに置き換える必要があります。また、ソリューション内の `DoyaChat` クラウド プロジェクトは不要になります。代わりに、EC2 上に Windows Server 2012 を展開した後、`WFE` プロジェクトをビルドしたものを含むディレクトリをホームとして指定して、新規 Web サイトを IIS 上に構成してください。
