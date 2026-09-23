import { HeartPulse } from "lucide-react";

function Footer() {
  return (
    <footer className="app-footer">
      <div className="footer-brand">
        <HeartPulse size={15} />
        <span>MediCore HIS</span>
      </div>

      <span>
        © 2026 MediCore Healthcare Systems
      </span>

      <span className="footer-status">
        <span />
        All systems operational
      </span>
    </footer>
  );
}

export default Footer;