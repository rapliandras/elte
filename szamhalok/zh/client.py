import socket
import struct

connection = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_address = ('localhost', 10001)
connection.connect(server_address)

connection2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
connection2.connect(server_address)

values = ("1,0,0,1,1,0")
values2 = ("1,0,0,1,0,1")
connection.sendall(values)
connection2.sendall(values2)

result = connection.recv(40)
result2 = connection2.recv(40)


print result
print result2

connection.close()
