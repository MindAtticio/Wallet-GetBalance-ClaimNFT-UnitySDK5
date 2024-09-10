using UnityEngine;
using UnityEngine.UI;
using Thirdweb;
using TMPro;
using Thirdweb.Unity;

public class ConnectWalletManagerStripped : MonoBehaviour
{
    [SerializeField] private ulong ActiveChainId = 80002;

    [field: SerializeField, Header("Buttons")] private Button WalletConnectButton;
    [SerializeField] private Button GetBalanceButton;
    [SerializeField] private Button ClaimNFTButton;

    [field: SerializeField, Header("Contract Information")] private string contractAddress = "0xCF3abe7795cbC0D1f591F768cA296F7658A37568";
    [SerializeField] private int tokenId = 0;

    [field: SerializeField, Header("Logs")] private TMPro.TextMeshProUGUI textStatus;

    private string currentWallet; // Variable to store the connected wallet address
    private IThirdwebWallet wallet;

    private void Awake()
    {
        WalletConnectButton.onClick.RemoveAllListeners();
        //WalletConnectButton.onClick.AddListener(() => ConnectWallet());
        WalletConnectButton.onClick.AddListener(ConnectWallet);

        GetBalanceButton.onClick.RemoveAllListeners();
        GetBalanceButton.onClick.AddListener(GetNFTBalance);

        ClaimNFTButton.onClick.RemoveAllListeners();
        ClaimNFTButton.onClick.AddListener(ClaimNFT);
    }

    private async void ConnectWallet()
    {
        WalletOptions options = new WalletOptions(provider: WalletProvider.WalletConnectWallet, chainId: ActiveChainId);
        textStatus.text = "Pending Connection...";
        try
        {
            wallet = await ThirdwebManager.Instance.ConnectWallet(options);
            currentWallet = await wallet.GetAddress(); // Store the wallet address
            Debug.Log("Wallet connected: " + currentWallet);
            textStatus.text = currentWallet; // Display the wallet address
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to connect wallet: " + ex.Message);
            textStatus.text = "Connection failed";
        }
    }

    public async void GetNFTBalance()
    {
        if (string.IsNullOrEmpty(currentWallet))
        {
            textStatus.text = "No wallet connected";
            return;
        }

        textStatus.text = "Getting balance...";
        try
        {
            var contract = await ThirdwebManager.Instance.GetContract(contractAddress, ActiveChainId);
            var balance = await contract.ERC1155_BalanceOf(currentWallet, tokenId); // Assuming the token ID is 0
            textStatus.text = "Balance: " + balance.ToString();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to get balance: " + ex.Message);
            textStatus.text = "Failed to get balance";
        }
    }
    public async void ClaimNFT()
    {
        if (string.IsNullOrEmpty(currentWallet))
        {
            textStatus.text = "No wallet connected";
            return;
        }

        textStatus.text = "Claiming NFT...";
        try
        {
            var contract = await ThirdwebManager.Instance.GetContract(contractAddress, ActiveChainId);
            var claim = await contract.DropERC1155_Claim(wallet, currentWallet, tokenId, 1);

            textStatus.text = "Transaction submitted, checking for confirmation...";
            

            textStatus.text = "Claim Succesfull!";
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to Claim: " + ex.ToString());
            textStatus.text = "Failed to Claim";
        }
    }  
}   


