import React from "react";
import { Link } from "react-router-dom";
import Card from "../UI/Card";

import styles from "./LoginForm.module.css";

const LoginForm = () => {
  return (
    <div className={styles["form-container"]}>
      <Card className="bg-blue">
        <h1>Login</h1>
        <form>
          <div className={styles.control}>
            <label htmlFor="email">E-mail</label>
            <input id="email"></input>
          </div>
          <div className={styles.control}>
            <label htmlFor="password">Password</label>
            <input id="password"></input>
          </div>
          <br />
          <span>-OR-</span>
          <Link>Create new account</Link>
          <button type="submit">Login</button>
        </form>
      </Card>
    </div>
  );
};

export default LoginForm;
