Este é um curto guia para aqueles que resolverem testar este plugin. Valhe lembrar que este programa ainda está em desenvolvimento, 
portanto alguns recursos podem estar quebrados, além da implementação ainda não ter sido terminada ainda. Uma pequena lista do que já foi
implementado pode ser vista abaixo:

Recursos implementados:

- Todas as classes de comunicação UDP implementadas, incluindo APIs simples para broadcast de mensagens.
- Primeiro estado do protocolo implementado e completamente funcional. (hosts conseguem se enxergar na rede, mais detalhes na monografia
associada ao projeto)

Como usar:

- Basta criar um GameObject vazio e associar a ele um component chamado P2PNetworking. Não esqueça de definir a porta para o canal
de comunicação de broadcast através do inspector da unity.
- Por padrão, o plugin iniciará no Modo StanBy. Isto significa que ele já estará escutando por outros peers conectados à rede.
- Para obter uma lista com todos os peers encontrados acessar o atributo público knownHosts na instancia ativa classe P2PNetworking. 
Para facilitar acesso a esta instância, P2PNetworking possui um atributo pulic chamado instance, que guardará uma refêrencia à mesma.
