import React from "react";
import { Link } from "react-router-dom";

import styles from "./Header.module.css";

const Header = () => {
  return (
    <header className={`bg-blue ${styles.header}`}>
      <Link to="/">
        <img src="/ap_icon_white.png" alt="Autoplace icon" />
      </Link>
      <nav>
        <ul>
          <li>
            <Link to="/identity">Login</Link>
          </li>
        </ul>
      </nav>
    </header>
  );
};

export default Header;
