import React from "react";

import styles from "./Footer.module.css";

const Footer = () => {
  return (
    <footer className={styles.footer}>
      <div className={styles["footer__container"]}>
        <span>
          &#169; {new Date().getFullYear()} - AutoPlace - Privacy - Contact Us
        </span>
      </div>
    </footer>
  );
};

export default Footer;
