# Mensajeria
Chat de consola desarrollado en C#
Este codigo se probo en windows, siga los siguiente pasos.
1. Abrá el repositorio https://github.com/cluis11/Mensajeria.git y seleccione la branch dev
2. Descarge el repositorio en una carpera
3. Ejecute dos instancias del powershell
4. En cada una utilizando cd abra la carpera donde guardo el repositorio
5. Una vez en la carpera ejecute dotnet build, solo en una de las instancias del powershell
6. En una de las instancias ejecute dotnet run -- <port>, donde <port> puede ser un puerto como 1515
7. Ejecute dotnet run -- <port> con un puerto diferente
8. El programa solicita la IP a donde quiere enviar el mensaje y luego el puerto, si ambos son correctos envia el mensaje
9. Luego de enviar o recibir un mensaje debe ingresar la IP, puerto o mensaje depende del punto en el que se encontraba al recibir el mensaje
