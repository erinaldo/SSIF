
/****** Object:  StoredProcedure [dbo].[SpCrearExistenciaPorBodega]    Script Date: 01/06/2017 03:19:24 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpCrearExistenciaPorBodega]
	-- Add the parameters for the stored procedure here
	@IDProducto AS CHAR(36)
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
		Bodega.IDBODEGA,
		@IDProducto AS IDPRODUCTO
	FROM
		Bodega
	WHERE
		Bodega.IDBODEGA NOT IN(
			SELECT
				Existencia.IDBODEGA
			FROM
				Existencia
				INNER JOIN Bodega ON Existencia.IDBODEGA = Bodega.IDBODEGA
			WHERE
				Existencia.IDPRODUCTO = @IDProducto
		)
END
