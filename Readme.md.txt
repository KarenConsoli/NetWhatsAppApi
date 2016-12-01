WhatsAppApi Windows
"""""""""""""""""""
1-Ir a la carpeta ...\WhatsAppApi1\Windows\

2-Ejecutar setup.exe

3-Listo! :D

WhatsAppApi Linux
"""""""""""""""""
1-Abrir Terminal y ejecutar:

sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list

sudo apt-get update

echo "deb http://download.mono-project.com/repo/debian wheezy-apache24-compat main" | sudo tee -a /etc/apt/sources.list.d/mono-xamarin.list


2-Abrir en terminal ...\WhatsAppApi1\AxolotlTestApp-master\WhatsTest\bin\Debug

3-Escribir:
mono WhatsTest.exe

4-Listo! :D