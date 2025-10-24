export function initRoot() {
    var portal = document.body.getElementsByClassName("rya-root-popup-container")[0];
    if (portal) return portal;

    portal = document.createElement("div");
    portal.classList.add("rya-root-popup-container");
    portal.classList.add("position-absolute");
    portal.classList.add("invisible");
    portal.classList.add("vw-100");
    portal.classList.add("vh-100");
    portal.classList.add("overflow-hidden");
    portal.classList.add("top-0");
    document.body.appendChild(portal);
    return portal;
}