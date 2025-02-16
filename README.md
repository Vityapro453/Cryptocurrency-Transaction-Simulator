Features of the Code:
User Wallets: Each user has a wallet with a balance.
Public-Private Keys: Transactions are signed with a private key and verified using a public key.
Transaction Validation: Ensures the sender has sufficient funds.
Blockchain Ledger: Transactions are recorded in a blockchain-like structure.


How It Works
Wallets:

Each user (Alice, Bob) has a wallet with a public-private key pair.
Each wallet starts with 100 tokens by default.
Transaction Creation & Signing:

Transactions are digitally signed with a private key.
A signature ensures authenticity and prevents tampering.
Blockchain Ledger:

Transactions are stored in a simple ledger (list of transactions).
When a transaction is added, the sender’s and recipient’s balances are updated.
Validation:

The system ensures sufficient balance before allowing a transaction.
The digital signature is verified to prevent unauthorized transactions.


Example Output

Alice's Balance: 100
Bob's Balance: 100
Transaction Success: 50 tokens sent.
Transaction Success: 20 tokens sent.
Alice's Final Balance: 70
Bob's Final Balance: 130
