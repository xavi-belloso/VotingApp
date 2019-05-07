curl http://localhost:5000/api/voting \
--request 'GET' 

echo '\n'

curl http://localhost:5000/api/voting \
--request 'POST' \
--data '["c#","java"]' \
--header 'Content-Type: application/json'

echo '\n'

curl http://localhost:5000/api/voting \
--request 'PUT' \
--data '"java"' \
--header 'Content-Type: application/json'

echo '\n'

winner=$(curl http://localhost:5000/api/voting \
--request 'DELETE' \
--silent \
--header 'Content-Type: application/json' | jq -r '.winner')

if [ "$winner" == "java" ]; then
    echo "PASSED!"
    exit 0
else
    echo "FAILED!"
    exit 1
fi

echo "The winner is :" $winner