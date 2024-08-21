#!/bin/bash
set -euo pipefail # Exit on error, unset variable or pipe fail

# Makes sure we can access the commands we need
for cmd in dotnet-ef; do
    command -v "$cmd" &> /dev/null || { echo "$cmd not found in path"; exit 1; }
done

PROJECT_DIR="$(cd "$(dirname "$0")" && cd .. && pwd)"
MIGRATIONS_DIR="$PROJECT_DIR/src/MagicSystem/Infrastructure/Database/Migrations"

# Don't do this stuff when your system runs in prod
read -p "This will set up a new database, do you want to start from a new initial migration or keep the old ones (new-initial/keep/stop): " response
if [[ "$response" == "new-initial" ]]; then
    dotnet-ef database drop --force --project $PROJECT_DIR/src/MagicSystem
    [ -d $MIGRATIONS_DIR ] && rm -rf "$MIGRATIONS_DIR"
    dotnet-ef migrations add initial --project $PROJECT_DIR/src/MagicSystem --context MagicSystemContext -o $MIGRATIONS_DIR
    dotnet-ef database update --project $PROJECT_DIR/src/MagicSystem
    echo "You can now use a clean database with your new initial migration"
elif [[ "$response" == "keep" ]]; then
    dotnet-ef database drop --force --project $PROJECT_DIR/src/MagicSystem
    dotnet-ef database update --project $PROJECT_DIR/src/MagicSystem
    echo "Wiped the database and applied existing migrations"
fi

exit 0