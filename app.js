var scene = new THREE.Scene();
var camera = new THREE.PerspectiveCamera( 75, window.innerWidth / window.innerHeight, 0.1, 1000 );
var renderer = new THREE.WebGLRenderer();
renderer.setSize( window.innerWidth, window.innerHeight );
document.body.appendChild( renderer.domElement );


var GRID_SIZE = 1;

var geometry = new THREE.BufferGeometry();

//var material = new THREE.MeshBasicMaterial( { color: 0x00ff00 } );
//scene.add( tri, material );
//camera.position.z = 5;

//function render() {
//    requestAnimationFrame( render );
//    renderer.render( scene, camera );
//}
//render();

function generateBlock() {

}