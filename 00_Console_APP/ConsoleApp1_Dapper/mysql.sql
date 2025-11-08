-- Active: 1700695206915@@localhost@3307@pms
-- Creazione della tabella roles
CREATE TABLE roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL
) ENGINE=InnoDB;

-- Creazione della tabella project_categories
CREATE TABLE project_categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

-- Creazione della tabella users
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    password VARCHAR(255) NOT NULL,
    email VARCHAR(100) NOT NULL,
    role_id INT,
    FOREIGN KEY (role_id) REFERENCES roles(id)
) ENGINE=InnoDB;

-- Creazione della tabella projects
CREATE TABLE projects (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    start_date DATE,
    end_date DATE,
    category_id INT,
    manager_id INT,
    FOREIGN KEY (category_id) REFERENCES project_categories(id),
    FOREIGN KEY (manager_id) REFERENCES users(id)
) ENGINE=InnoDB;

-- Creazione della tabella tasks
CREATE TABLE tasks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    description TEXT,
    status VARCHAR(50),
    priority VARCHAR(50),
    due_date DATE,
    project_id INT,
    assigned_to_user_id INT,
    FOREIGN KEY (project_id) REFERENCES projects(id),
    FOREIGN KEY (assigned_to_user_id) REFERENCES users(id)
) ENGINE=InnoDB;

-- Creazione della tabella task_attachments
CREATE TABLE task_attachments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    file_name VARCHAR(255) NOT NULL,
    file_data BLOB,
    task_id INT,
    FOREIGN KEY (task_id) REFERENCES tasks(id)
) ENGINE=InnoDB;
