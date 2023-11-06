CREATE PROCEDURE GetRandomCurrencyValues
    @Year INT
AS
BEGIN
    -- Create a table to store the random currency values
    CREATE TABLE #RandomCurrencyValues (
        Month INT,
        CurrencyValue DECIMAL(19, 4) -- Assuming a scale of 4 decimal places for currency
    )

    -- Seed the random number generator with the input year
    -- to generate a consistent set of random numbers for each year
    DECLARE @Seed INT = @Year

    -- Insert 12 random currency values into the temporary table
    DECLARE @MonthCounter INT = 1
    WHILE @MonthCounter <= 12
    BEGIN
        INSERT INTO #RandomCurrencyValues (Month, CurrencyValue)
        VALUES (
            @MonthCounter,
            CAST(RAND(CHECKSUM(NEWID(), @Seed)) * 100000 AS DECIMAL(19, 4)) -- Generate values up to 100,000 for illustration
        )
        SET @MonthCounter = @MonthCounter + 1
    END

    -- Select the random currency values
    SELECT Month, CurrencyValue FROM #RandomCurrencyValues

    -- Drop the temporary table
    DROP TABLE #RandomCurrencyValues
END
