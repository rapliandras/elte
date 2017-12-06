import socket
import struct
import operator
import sys
import select

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_address = ('localhost', 10001)
sock.bind(server_address)


inputs = [ sock ]
outputs = []
timeout = 1

while inputs:
    try:
        readable, writable, exceptional = select.select(inputs, outputs, inputs, timeout)


        sock.listen(5)

        inventory = "4,3,2,1,0"
        inventory_list = inventory.split(",")

        connection, client_address = sock.accept()

        data = connection.recv(16)
        data_list = data.split(",")
    
        result = ""
        for i in [0,1,2,3,4]:
            if int(data_list[i]) == 0:
                result += "0,"
            elif int(inventory_list[i]) >= int(data_list[i]) and int(data_list[i]) != 0:
                result += "1,"
            else:
                result += "-1,"

        s = 0
        for i in data_list[0:-1]:
            s += int(i)


        if s%2 == 0 and int(data_list[5]) == 1:
            result +=  "Nem serult"
        else:
            result += "Serult"

        connection.sendall(result)
        connection2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        server_address = ('localhost', 10004)
        connection2.connect(server_address)
        connection2.sendall(inventory)
        print connection2.recv(1024)

        connection2.close()


    except KeyboardInterrupt:
        print "interrupted"
        sock.close()
        quit()


