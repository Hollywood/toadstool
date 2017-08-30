set-executionpolicy remotesigned

$webConfig = 'Add path to your webconfig file'
$doc = (Get-Content $webConfig) -as [Xml]
$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'MS_WebHookReceiverSecret_GitHub'}
$obj.value = 'Insert Org WebHook Secret Here'

$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'GitHubAccessToken'}
$obj.value = 'Insert PAT Here'

$doc.Save($webConfig)