
let scene = new THREE.Scene();
let camera = new THREE.PerspectiveCamera( 75, window.innerWidth / window.innerHeight, 0.1, 1000 );
let renderer = new THREE.WebGLRenderer();
renderer.setSize( window.innerWidth, window.innerHeight );
document.body.appendChild( renderer.domElement );


let GRID_SIZE = 10;

let geometry = new THREE.BufferGeometry();


let material = new THREE.MeshBasicMaterial( { color: 0x00ff00 } );
scene.add( tri, material );
camera.position.z = 5;

function render() {
    requestAnimationFrame( render );
    renderer.render( scene, camera );
}
render();

function generateBlock() {
//
}

