/opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P LojaSenha@247 -d master -i /tmp/init-lojadb.sql
/opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P LojaSenha@247 -d master -i /tmp/init-users-roles.sql
/opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P LojaSenha@247 -d master -i /tmp/insert-user-roles.sql