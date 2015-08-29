# Lyra_version.zip 作成スクリプト
# TODO: 拡張指定のみでコピーできるように(*.dll, *.exe, *.config)

# ディレクトリ作成
New-Item artifacts\Lyra -ItemType Directory

# コピー
Copy-Item Lyra\bin\Debug\* artifacts\Lyra -Recurse

# ゴミが生成されるのは、ルートのみなのでいい感じに消す
Get-ChildItem artifacts\Lyra\ -Include *.pdb,*.xml -Recurse | Remove-Item