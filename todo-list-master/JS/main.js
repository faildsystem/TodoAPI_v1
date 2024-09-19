// Selectors
const toDoInput = document.querySelector('.todo-input');
const toDoBtn = document.querySelector('.todo-btn');
const toDoList = document.querySelector('.todo-list');
const standardTheme = document.querySelector('.standard-theme');
const lightTheme = document.querySelector('.light-theme');
const darkerTheme = document.querySelector('.darker-theme');

// Event Listeners
toDoBtn.addEventListener('click', addToDo);
toDoList.addEventListener('click', deletecheck);
document.addEventListener("DOMContentLoaded", getAllTodos);
standardTheme.addEventListener('click', () => changeTheme('standard'));
lightTheme.addEventListener('click', () => changeTheme('light'));
darkerTheme.addEventListener('click', () => changeTheme('darker'));

// Check if one theme has been set previously and apply it (or standard theme if not found):
let savedTheme = localStorage.getItem('savedTheme');
changeTheme(savedTheme || 'standard');

// Function to add a new to-do
async function addToDo(event) {
    event.preventDefault();

    if (toDoInput.value.trim() === '') {
        alert("You must write something!");
        return;
    }

    // Create to-do element
    const toDoDiv = document.createElement("div");
    toDoDiv.classList.add('todo', `${savedTheme}-todo`);

    const newToDo = document.createElement('li');
    newToDo.innerText = toDoInput.value;
    newToDo.classList.add('todo-item');
    toDoDiv.appendChild(newToDo);

    // Check and delete buttons
    const checked = document.createElement('button');
    checked.innerHTML = '<i class="fas fa-check"></i>';
    checked.classList.add('check-btn', `${savedTheme}-button`);
    toDoDiv.appendChild(checked);

    const deleted = document.createElement('button');
    deleted.innerHTML = '<i class="fas fa-trash"></i>';
    deleted.classList.add('delete-btn', `${savedTheme}-button`);
    toDoDiv.appendChild(deleted);

    // Append to the list
    toDoList.appendChild(toDoDiv);

    // Clear the input field
    toDoInput.value = '';

    // Save to the server
    const options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            title: toDoInput.value,
            description: "",
            isCompleted: false
        })
    };
    await saveTodo(options);
}

// Function to save to-do to the server
async function saveTodo(options) {
    try {
        const response = await fetch('https://localhost:7140/Todo/Add', options);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.json();
        console.log('Success:', data);
    } catch (error) {
        console.error('Error:', error);
    }
}

// Function to handle delete and check actions
// Function to handle delete and check actions
async function deletecheck(event) {
    const item = event.target;

    if (item.classList.contains('delete-btn')) {
        const toDoDiv = item.parentElement;
        toDoDiv.classList.add("fall");

        // Extract the to-do ID or other identifier if needed
        const todoId = toDoDiv.dataset.id;

        // Delete from the server
        await removeTodoFromServer(todoId);

        // Remove from the UI after animation
        toDoDiv.addEventListener('transitionend', function () {
            toDoDiv.remove();
        });
    }

    if (item.classList.contains('check-btn')) {
        item.parentElement.classList.toggle("completed");
    }
}

// Function to remove a to-do from the server
async function removeTodoFromServer(id) {
    try {
        const response = await fetch(`https://localhost:7140/Todo/Delete/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        console.log('To-do deleted successfully');
    } catch (error) {
        console.error('Error:', error);
    }
}

// Function to load all to-dos from the server
async function getAllTodos() {
    try {
        const response = await fetch('https://localhost:7140/Todo/GetAll');
        const todos = await response.json();

        todos.forEach(todo => {
            const toDoDiv = document.createElement("div");
            toDoDiv.classList.add("todo", `${savedTheme}-todo`);
            toDoDiv.dataset.id = todo.id; // Add the ID as a data attribute

            const newToDo = document.createElement('li');
            newToDo.innerText = `${todo.title}\n\n${todo.description}`;
            newToDo.classList.add('todo-item');
            toDoDiv.appendChild(newToDo);

            // Check and delete buttons
            const checked = document.createElement('button');
            checked.innerHTML = '<i class="fas fa-check"></i>';
            checked.classList.add("check-btn", `${savedTheme}-button`);
            toDoDiv.appendChild(checked);

            const deleted = document.createElement('button');
            deleted.innerHTML = '<i class="fas fa-trash"></i>';
            deleted.classList.add("delete-btn", `${savedTheme}-button`);
            toDoDiv.appendChild(deleted);

            // Append to the list
            toDoList.appendChild(toDoDiv);

            // Mark as completed if needed
            if (todo.isCompleted) {
                toDoDiv.classList.add('completed');
            }
        });
    } catch (error) {
        console.error('Error:', error);
    }
}


// Function to load all to-dos from the server
async function getAllTodos() {
    try {
        const response = await fetch('https://localhost:7140/Todo/GetAll');
        const todos = await response.json();

        todos.forEach(todo => {
            const toDoDiv = document.createElement("div");
            toDoDiv.classList.add("todo", `${savedTheme}-todo`);

            const newToDo = document.createElement('li');
            newToDo.innerText = `${todo.title}\n\n${todo.description}`;
            newToDo.classList.add('todo-item');
            toDoDiv.appendChild(newToDo);

            // Check and delete buttons
            const checked = document.createElement('button');
            checked.innerHTML = '<i class="fas fa-check"></i>';
            checked.classList.add("check-btn", `${savedTheme}-button`);
            toDoDiv.appendChild(checked);

            const deleted = document.createElement('button');
            deleted.innerHTML = '<i class="fas fa-trash"></i>';
            deleted.classList.add("delete-btn", `${savedTheme}-button`);
            toDoDiv.appendChild(deleted);

            // Append to the list
            toDoList.appendChild(toDoDiv);

            // Mark as completed if needed
            if (todo.isCompleted) {
                toDoDiv.classList.add('completed');
            }
        });
    } catch (error) {
        console.error('Error:', error);
    }
}

// Function to change theme
function changeTheme(color) {
    localStorage.setItem('savedTheme', color);
    savedTheme = color;

    document.body.className = color;
    document.getElementById('title').classList.toggle('darker-title', color === 'darker');
    document.querySelector('input').className = `${color}-input`;

    document.querySelectorAll('.todo').forEach(todo => {
        todo.className = `todo ${color}-todo${todo.classList.contains('completed') ? ' completed' : ''}`;
    });

    document.querySelectorAll('button').forEach(button => {
        if (button.classList.contains('check-btn')) {
            button.className = `check-btn ${color}-button`;
        } else if (button.classList.contains('delete-btn')) {
            button.className = `delete-btn ${color}-button`;
        } else if (button.classList.contains('todo-btn')) {
            button.className = `todo-btn ${color}-button`;
        }
    });
}
