-- Parte 2 do Desafio Técnico

SELECT CAP.Numero, P.Nome AS NomeFornecedor, CAP.DataVencimento, NULL AS DataPagamento,
       (CAP.Valor + CAP.Acrescimo - CAP.Desconto) AS ValorLiquido, 'A PAGAR' AS Situacao
FROM ContasAPagar CAP
INNER JOIN Pessoas P ON CAP.CodigoFornecedor = P.Codigo
UNION ALL
SELECT CP.Numero, P.Nome AS NomeFornecedor, CP.DataVencimento, CP.DataPagamento,
       (CP.Valor + CP.Acrescimo - CP.Desconto) AS ValorLiquido, 'PAGA' AS Situacao
FROM ContasPagas CP
INNER JOIN Pessoas P ON CP.CodigoFornecedor = P.Codigo
ORDER BY Numero;
