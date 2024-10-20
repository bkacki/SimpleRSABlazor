using System.Text.Json;
using Microsoft.JSInterop;

namespace SimpleRSABlazor.Services
{
    public class RSAKeyStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public RSAKeyStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<List<string>> GetSavedKeyNamesAsync()
        {
            return await GetSavedKeyNamesAsync("keyNames");
        }

        public async Task<List<string>> GetSavedPublicKeyNamesAsync()
        {
            return await GetSavedKeyNamesAsync("publicKeyNames");
        }

        public async Task<List<string>> GetSavedPrivateKeyNamesAsync()
        {
            return await GetSavedKeyNamesAsync("privateKeyNames");
        }

        private async Task<List<string>> GetSavedKeyNamesAsync(string storageKey)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", storageKey);

            if (string.IsNullOrEmpty(json))
            {
                return new List<string>();
            }

            return JsonSerializer.Deserialize<List<string>>(json);
        }

        public async Task SaveKeyPairAsync(string keyName, string publicKey, string privateKey)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"{keyName}_public", publicKey);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"{keyName}_private", privateKey);
            await UpdateKeyNames(keyName, "keyNames");
        }

        public async Task<string> GetPublicKeyAsync(string keyName)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", $"{keyName}_public");
        }

        public async Task<string> GetPrivateKeyAsync(string keyName)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", $"{keyName}_private");
        }

        public async Task SavePublicKeyAsync(string keyName, string publicKey)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"{keyName}_public", publicKey);
            await UpdateKeyNames(keyName, "publicKeyNames");
        }

        public async Task SavePrivateKeyAsync(string keyName, string privateKey)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"{keyName}_private", privateKey);
            await UpdateKeyNames(keyName, "privateKeyNames");
        }

        public async Task RemoveKeyPairAsync(string keyName)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", $"{keyName}_public");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", $"{keyName}_private");
            await RemoveKeyNameAsync(keyName, "keyNames");
        }

        private async Task UpdateKeyNames(string keyName, string storageKey)
        {
            var names = await GetSavedKeyNamesAsync(storageKey);
            if (!names.Contains(keyName))
            {
                names.Add(keyName);
                await SaveKeyNamesAsync(names, storageKey);
            }
        }

        private async Task RemoveKeyNameAsync(string keyName, string storageKey)
        {
            var keyNames = await GetSavedKeyNamesAsync(storageKey);
            keyNames.Remove(keyName);
            await SaveKeyNamesAsync(keyNames, storageKey);
        }

        public async Task SaveKeyNamesAsync(List<string> keyNames, string storageKey)
        {
            var json = JsonSerializer.Serialize(keyNames);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", storageKey, json);
        }
    }
}
