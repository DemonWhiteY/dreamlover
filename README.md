# DreamLover 基于Unity和智谱AI的虚拟助手

### 快速部署
``` bash
git clone https://github.com/DemonWhiteY/dreamlover.git
cd server
pip install -r requirements.txt
```

然后点击`./server`文件夹中的`.env`文件，将其中的`ZHI_PU_API`换成你自己的API码。

[智谱AI开发平台](https://bigmodel.cn/)，点这里获得API密钥

如果你是第一次使用虚拟环境，记得先创建并激活一个虚拟环境，然后再安装依赖，以保持环境的隔离性：

```bash
# 创建虚拟环境
python -m venv venv

# 激活虚拟环境
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate

# 安装依赖
pip install -r requirements.txt

```

然后启动服务端
``` bash
python pythonserver.py
```
