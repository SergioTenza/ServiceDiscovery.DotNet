FROM mcr.microsoft.com/devcontainers/dotnet:0-7.0

RUN wget -O dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/764f2fec-710d-490d-a341-88636bce1a8d/35fc13fc20161a7d196200d9c2c6a8f0/dotnet-sdk-8.0.100-rc.1.23463.5-linux-x64.tar.gz \
    && dotnet_sha512='ac941fd16fd7c328f7cc44b132b4253ddb2b6a6c152af5f43c71c6cd0d468c89b7276ebf6c08895dcb6e5e25f7cae83b6fbacb91cfcc4a61d49b5657a834a901' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \   
    && export DOTNET_ROOT=$HOME/dotnet \
    && export PATH=$PATH:$HOME/dotnet 