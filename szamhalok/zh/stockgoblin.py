import socket
import struct
import operator
import sys
import select

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_address = ('localhost', 10004)
sock.bind(server_address)


inputs = [ sock ]
outputs = []
timeout = 1

while inputs:
    try:
        readable, writable, exceptional = select.select(inputs, outputs, inputs, timeout)


        sock.listen(5)

        connection, client_address = sock.accept()

        data = connection.recv(16)
        data_list = data.split(",")
    
        connection.sendall("ACK")
    except KeyboardInterrupt:
        print "interrupted"
        sock.close()
        quit()


