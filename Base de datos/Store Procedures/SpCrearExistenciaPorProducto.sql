
/****** Object:  StoredProcedure [dbo].[SpCrearExistenciaPorProducto]    Script Date: 01/06/2017 03:22:57 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpCrearExistenciaPorProducto]
	-- Add the parameters for the stored procedure here
	@IDBodega AS CHAR(36)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Existencia
	(
		IDEXISTENCIA,
		Reg,
		CANTIDAD,
		CONSIGNADO,
		IDBODEGA,
		IDPRODUCTO
	)
	SELECT
		NEWID(),
		GETDATE() AS Reg,
		0 AS CANTIDAD,
		0 AS CONSIGNADO,
		@IDBodega AS IDBODEGA,
		Producto.IDPRODUCTO
	FROM
		Producto
	WHERE
		Producto.IDPRODUCTO NOT IN(
			SELECT
				Existencia.IDPRODUCTO
			FROM
				Existencia
				INNER JOIN Producto ON Existencia.IDPRODUCTO = Producto.IDPRODUCTO
			WHERE
				Existencia.IDBODEGA = @IDBodega
		)
END
