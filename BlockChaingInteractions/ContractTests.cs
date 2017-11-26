namespace BlockChaingInteractions
{
    using Nethereum.Web3;
    using Xunit;

    public class ContractTests
    {
        private Web3 _web3 = null;
        private string _bytecode = "60606040523415600e57600080fd5b609b8061001c6000396000f300606060405260043610603e5763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663165c4a1681146043575b600080fd5b3415604d57600080fd5b6059600435602435606b565b60405190815260200160405180910390f35b02905600a165627a7a72305820394b94e77c25c91d96ea4fd6d40ce6971d8d6c5006a2bf8ec05c8fdb92ca9e720029";
        private string _creatorAddress = "0x6fab9ac64a2c86580f0c74478d8fcd175938d5a8";
        private string _abi = "[{\"constant\":true,\"inputs\":[{\"name\":\"firstValue\",\"type\":\"uint256\"},{\"name\":\"secondValue\",\"type\":\"uint256\"}],\"name\":\"multiply\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"}]";

        public ContractTests()
        {
            _web3 = new Web3("http://localhost:8545");
        }

        [Theory,
            InlineData("0x6fab9ac64a2c86580f0c74478d8fcd175938d5a8", "password", 2100000)]
        public async void DeployContract_ShouldReturnTransactionHash(
            string creatorAddress,
            string password,
            int gas)
        {
            var unlockResult = await _web3
                .Personal
                .UnlockAccount
                .SendRequestAsync(creatorAddress, password, 60);

            _web3.TransactionManager.DefaultGas = gas;

            var transactionHashResult = await _web3
                .Eth
                .DeployContract
                .SendRequestAsync(_abi, _bytecode, _creatorAddress);

            Assert.False(string.IsNullOrWhiteSpace(transactionHashResult));
        }

        [Theory, 
            InlineData("0xb766e7546e8dbd051259f84ddd824504d1f4bba220a0891f083e6694ae454b3b")]
        public async void CallMultiplyFunction_ShouldReturn50(string transactionHash)
        {
            var transactionResult = await _web3
                .Eth
                .Transactions
                .GetTransactionReceipt
                .SendRequestAsync(transactionHash);

            var contractAddress = transactionResult.ContractAddress;

            Assert.NotNull(contractAddress);

            var getContractResult = _web3.Eth.GetContract(_abi, contractAddress);
            var multiplyFunction = getContractResult.GetFunction("multiply");
            var multiplyResult = await multiplyFunction.CallAsync<int>(5, 10);

            Assert.Equal(50, multiplyResult);
        }
    }
}
