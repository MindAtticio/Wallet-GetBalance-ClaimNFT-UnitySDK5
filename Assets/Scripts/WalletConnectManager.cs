using System.Collections.Generic;
using Newtonsoft.Json;
using Thirdweb;
using Thirdweb.Unity;
using Thirdweb.Unity.Examples;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ConnectWalletManager : MonoBehaviour
{
    [field: SerializeField, Header("Wallet Options")]
    private ulong ActiveChainId = 80002;

    [field: SerializeField, Header("Connect Wallet")] private Button WalletConnectButton;

    private void Awake()
    {
        InitializePanels();
    }

    private void InitializePanels()
    {
        WalletConnectButton.onClick.RemoveAllListeners();
        WalletConnectButton.onClick.AddListener(() =>
        {
            var options = GetWalletOptions(WalletProvider.WalletConnectWallet);
            ConnectWallet(options);
        });
    }

    private async void ConnectWallet(WalletOptions options)
    {
        // Connect the wallet

        var internalWalletProvider = options.Provider == WalletProvider.MetaMaskWallet ? WalletProvider.WalletConnectWallet : options.Provider;
        

        var wallet = await ThirdwebManager.Instance.ConnectWallet(options);

        // Initialize the wallet panel

    }

    private WalletOptions GetWalletOptions(WalletProvider provider)
    {
        switch (provider)
        {
            case WalletProvider.PrivateKeyWallet:
                return new WalletOptions(provider: WalletProvider.PrivateKeyWallet, chainId: ActiveChainId);
            case WalletProvider.InAppWallet:
                var inAppWalletOptions = new InAppWalletOptions(authprovider: AuthProvider.Google);
                return new WalletOptions(provider: WalletProvider.InAppWallet, chainId: ActiveChainId, inAppWalletOptions: inAppWalletOptions);
            case WalletProvider.WalletConnectWallet:
                var externalWalletProvider =
                    Application.platform == RuntimePlatform.WebGLPlayer ? WalletProvider.MetaMaskWallet : WalletProvider.WalletConnectWallet;
                return new WalletOptions(provider: externalWalletProvider, chainId: ActiveChainId);
            default:
                throw new System.NotImplementedException("Wallet provider not implemented for this example.");
        }
    }

}

