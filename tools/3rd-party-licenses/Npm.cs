using System.Net.Http.Json;
using System.Text.Json;

namespace _3rd_party_licenses;

public class Npm(string[] npmFolders)
{
    public async Task<List<DependencyInfo>> GetPackagesForPackageJson()
    {
        var list = new List<DependencyInfo>();

        foreach (var npmFolder in npmFolders)
        {
            Console.WriteLine($"Getting packages for {npmFolder}");

            await Runner.ExecuteCommand(npmFolder, "npm", "install");
            var result = await Runner.ExecuteCommand(npmFolder, "npm", "ls --json --depth 0");

            var resultJson = JsonSerializer.Deserialize<NpmResult>(result,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var client = new HttpClient();
            foreach (var resultJsonDependency in resultJson.Dependencies)
            {
                var json = await client.GetFromJsonAsync<NpmRegistry>(resultJsonDependency.Value.Resolved.Split("/-/")[0]);
                string licenseUrl = json.License switch
                {
                    "MIT" => "https://opensource.org/licenses/MIT",
                    "Apache-2.0" => "https://opensource.org/licenses/Apache-2.0",
                    "ISC" => "https://opensource.org/licenses/ISC",
                    "BSD-3-Clause" => "https://opensource.org/licenses/BSD-3-Clause",
                    _ => throw new Exception(json.License)
                };
                list.Add(new DependencyInfo {
                    RegistryUrl = $"https://www.npmjs.com/package/{resultJsonDependency.Key}",
                    License = json.License,
                    ProjectUrl = json.Homepage,
                    Id = resultJsonDependency.Key,
                    LicenseUrl = licenseUrl });
            }
        }

        return list;
    }
}

class NpmRegistry
{
    public string License { get; set; }
    public string Homepage { get; set; }
}
class NpmResult
{
    public Dictionary<string, Dependency> Dependencies { get; set; }
}

class Dependency
{
    public string Resolved { get; set; }
}
/*
 * {
   "version": "1.0.0",
   "name": "service-pulse",
   "dependencies": {
     "@eslint/js": {
       "version": "9.13.0",
       "resolved": "https://registry.npmjs.org/@eslint/js/-/js-9.13.0.tgz",
       "overridden": false
     },
     "@pinia/testing": {
       "version": "0.1.6",
       "resolved": "https://registry.npmjs.org/@pinia/testing/-/testing-0.1.6.tgz",
       "ove
*/