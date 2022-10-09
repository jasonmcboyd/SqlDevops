# System.Io, PowerShell, and relative paths do not play well together.
# Use this function to normalize any paths that could be relative paths.
function Normalize-Path {
    param (
        [string]
        $Path
    )

    $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Path)
}