import socket
import msvcrt
import sys
import random

if len(sys.argv) < 2:
	print('Usage: python keyboard.py IP PORT')
	sys.exit()

UDP_IP = sys.argv[1]
UDP_PORT = sys.argv[2]
UDP_PORT = int(UDP_PORT)

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
#sock.bind((UDP_IP, UDP_PORT))

while True:
    c = msvcrt.getch()
    if c == b'\x00':
    	continue
    if c == b'q':
        break
    print('end: ' + str(c))
    m = "0 255 0 0 0 255 0"
    if c == b'w':
        m = "0 255 255 255 0 0 0"
    elif c == b'e':
        m = "0 0 0 255 255 255 0"
    elif c == b'r':
        m = str(round(random.uniform(0,1))*255) + " " + str( round(random.uniform(0,1))*255) +" " +str( round(random.uniform(0,1))*255)+" " +str( round(random.uniform(0,1))*255)+" " +str( round(random.uniform(0,1))*255)+" " +str( round(random.uniform(0,1))*255)+" " +str( round(random.uniform(0,1))*255)
    c = m.encode('utf-8') 
    print('send: ' + str(c))

    sock.sendto(c, (UDP_IP, UDP_PORT))

sock.close()
