-- Create WalletTransactions table
CREATE TABLE IF NOT EXISTS WalletTransactions (
    Id UUID PRIMARY KEY,
    WalletId UUID NOT NULL REFERENCES Wallets(Id) ON DELETE CASCADE,
    PaymentId UUID REFERENCES Payments(Id) ON DELETE SET NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    BalanceAfter DECIMAL(10, 2) NOT NULL,
    Type VARCHAR(20) NOT NULL,
    Description TEXT,
    Reference VARCHAR(100),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

