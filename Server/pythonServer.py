# coding: utf-8
import os
import sys
from zhipuai import ZhipuAI
from langchain_openai import ChatOpenAI
from dotenv import find_dotenv, load_dotenv
from langchain_core.prompts import ChatPromptTemplate, MessagesPlaceholder
_ = load_dotenv(find_dotenv())
client = ZhipuAI(api_key=os.environ["ZHIPUAI_API_KEY"])
from langchain_openai import ChatOpenAI

# ----------------------------------------------------------情感分类模块----------------------------------------------------------------
from langchain_core.pydantic_v1 import BaseModel, Field

tagging_prompt = ChatPromptTemplate.from_template(
    """
请提供一段具体的文本内容以便我从中抽取信息。
仅提取“Classification”函数中提到的属性。

段落:
{input}
"""
)


class Classification(BaseModel):
    # sentiment 情绪
    aggressiveness: int = Field(description="文本的攻击性程度在1到10的范围内是如何的")
    response: str = Field(
        description="对于该文本回应可能产生的情绪用以下情绪代号回答：1	非常高兴，2	高兴，3	一般高兴，4	一般生气，5	生气，6	非常生气，7	有些伤心，8	伤心，9	非常伤心，10	吃惊"
    )


llm = ChatOpenAI(
    base_url="https://open.bigmodel.cn/api/paas/v4",
    api_key=os.environ["ZHIPUAI_API_KEY"],
    model="glm-4",
).with_structured_output(Classification)

tagging_chain = tagging_prompt | llm

# unity.Debug.Log("脚本加载2")
# ----------------------------------------------------------角色扮演模块----------------------------------------------------------------

def char_response(origin_text,initial_messages,meta_info):
    messages = initial_messages.copy()  # 初始化对话消息
    messages = messages[-10:]
    messages.append({"role": "user", "content": origin_text})
    response = client.chat.completions.create(
            model="charglm-3", meta=meta_info, messages=messages
        )
    bot_response = response.choices[0].message.content
    messages.append({"role": "assistant", "content": bot_response})
    emotion = tagging_chain.invoke({"input": origin_text}).response
    word = bot_response
    message = {"command": "echo", "response": word, "emotion": emotion}
    json_message = json.dumps(message)
    return json_message

# --------------------------------------------------------Python服务器搭建-----------------------------------------------------


import threading
import queue
import socket
import json
import sqlite3


message_queue = queue.Queue()
event = threading.Event()
exit_event=threading.Event()

def message_receiver():
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(("localhost", 12345))
    server_socket.listen(1)
    server_socket.settimeout(1.0) 
    conn = sqlite3.connect('../Demo/RIKO_Data/StreamingAssets/Database.db')
    cursor = conn.cursor()
    meta_info = updateMetaData(init(cursor),cursor)
    initial_messages = updateHistory(init(cursor),cursor)
    print("Waiting for connection...")
    event.set()
    while not exit_event.is_set():
      try: 
        client_socket, addr = server_socket.accept()
        print(f"Connection from {addr}")
        event.set()

        data = client_socket.recv(1024).decode()
        print(f"Received data: {data}")
        event.set()
        json_data = json.loads(data)
        
        
        if json_data.get("command")=="update":
             meta_info = updateMetaData(json_data.get("ask"),cursor)
             initial_messages = updateHistory(json_data.get("ask"),cursor)
             message = {"command": "echo", "response": meta_info.get("bot_info"), "emotion": "1"}
             json_message = json.dumps(message)
                     
             client_socket.sendall(json_message.encode("utf-8"))
             
        else:
            message = char_response(json_data.get("ask"),initial_messages,meta_info)
                        
            client_socket.sendall(message.encode("utf-8"))
            
            
        # else:
        
        

        client_socket.close()
      except socket.timeout:
            # 超时异常处理
            continue
      except Exception as e:
            # 处理其他可能的异常
            print(f"发生异常: {e}")
            break  
    
    server_socket.close()
    print("服务器线程退出")
    sys.exit()
receiver_thread = threading.Thread(target=message_receiver)
receiver_thread.start()


# unity.Debug.Log("脚本加载3")
# ----------------------------------------------------------连接数据库---------------------------------------------------------------


def updateHistory(Charname,cursor):
   

    Charname="'"+Charname+"'"
    query = f"SELECT Q_word,A_word FROM HistoryMessage WHERE Charname={Charname}"
    cursor.execute(query)

    rows = cursor.fetchall()
    messages=[]
    for row in rows:
         q_data={
                 "role": "user", "content":row[0]
         }
         a_data={"role": "assistant",
        "content": row[1]        
         }

         messages.append(q_data)
         messages.append(a_data)

    messages = messages[-10:]
    
    return messages

def updateMetaData(Charname,cursor):
    
    Charname="'"+Charname+"'"
    query = f"SELECT * FROM Character WHERE name={Charname}"
    cursor.execute(query)

    row = cursor.fetchone()


    meta_info = {
        "user_info": "我是妖孽白，是一个男性，是你的老板",
        "bot_info": "你的名字是"+row[1]+"个人职业和情况是"+row[3]+"年龄"+str(row[4])+"岁"+"性格:"+str(row[5])+"(越趋向0越高冷，越趋向1越暴躁，居中温柔),MBTI人格指标分别为：e-i:"+str(row[6])+",s-n:"+str(row[7])+",f-t:"+str(row[8])+",p-j:"+str(row[9])+"你要尽量扮演以上任务来与我对话，不要透露你是AI模型",
        "bot_name": row[1],
        "user_name": "妖孽白",
    }


    
    return meta_info

def init(cursor):
    query = f"SELECT * FROM Initinfo"
    cursor.execute(query)

    row = cursor.fetchone()

    id=row[1]

    query2 = f"SELECT name FROM Character WHERE id={id}"
    cursor.execute(query2)

    row2 = cursor.fetchone()

    name=row2[0]
    
    return name
    


#---------------------------------------------------服务器控制台----------------------------------------------------
def command_line_interface():
    def exit_command():
        print("Exiting...")
        event.set()
        exit_event.set()
        sys.exit()
       

    def find_command():
        print("Find command executed")
        event.set()
        return True

    def unknown_command(command):
        print(f"Unknown command: {command}")
        event.set()
        return True

    commands = {
        "exit": exit_command,
        "find": find_command,
    }

    while True:
        event.wait()
        command = input(">> ")
        command_func = commands.get(command, lambda: unknown_command(command))
        if not command_func():
            break

command_line_interface()
