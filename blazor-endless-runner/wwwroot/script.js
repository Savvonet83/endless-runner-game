// Simple canvas game adapted for Blazor index page
window.addEventListener('load', ()=>{
  const canvas = document.getElementById('game');
  if (!canvas) return;
  const ctx = canvas.getContext('2d');
  const scoreEl = document.getElementById('score');
  const startBtn = document.getElementById('start');
  const restartBtn = document.getElementById('restart');

  let w = canvas.width, h = canvas.height;
  let mouse = {x: w/2, y: h - 50};
  let player = {x: w/2, y: h - 40, r: 12};
  let obstacles = [];
  let running = false, score = 0, lastSpawn = 0;
  let last = Date.now();

  function spawn(){
    if (Date.now() - lastSpawn > 800){
      const size = 12 + Math.random()*24;
      obstacles.push({x: Math.random()*(w-size), y:-size, size, speed: 60+Math.random()*120});
      lastSpawn = Date.now();
    }
  }

  function update(dt){
    player.x += (mouse.x - player.x) * Math.min(1, dt*10);
    player.y += (mouse.y - player.y) * Math.min(1, dt*10);
    for (let i = obstacles.length-1; i>=0; i--){
      const o = obstacles[i];
      o.y += o.speed * dt;
      if (o.y > h + o.size) obstacles.splice(i,1);
      const dx = (o.x + o.size/2) - player.x;
      const dy = (o.y + o.size/2) - player.y;
      const dist = Math.hypot(dx,dy);
      if (dist < o.size/2 + player.r){ running = false; }
    }
  }

  function draw(){
    ctx.clearRect(0,0,w,h);
    ctx.fillStyle = '#4fe36b';
    ctx.beginPath(); ctx.arc(player.x, player.y, player.r, 0, Math.PI*2); ctx.fill();
    ctx.fillStyle = '#f25f5c';
    obstacles.forEach(o=> ctx.fillRect(o.x, o.y, o.size, o.size));
  }

  function loop(){
    const now = Date.now();
    const dt = Math.min(0.05, (now-last)/1000);
    last = now;
    if (running){ spawn(); update(dt); score += dt * 10; scoreEl.textContent = 'Score: ' + Math.floor(score); }
    draw();
    if (!running){ restartBtn.style.display = 'inline-block'; startBtn.style.display = 'none'; }
    requestAnimationFrame(loop);
  }

  canvas.addEventListener('mousemove', e => {
    const rect = canvas.getBoundingClientRect();
    mouse.x = (e.clientX - rect.left) * (canvas.width / rect.width);
    mouse.y = (e.clientY - rect.top) * (canvas.height / rect.height);
  });

  canvas.addEventListener('touchmove', e=>{ e.preventDefault(); const t=e.touches[0]; const rect=canvas.getBoundingClientRect(); mouse.x=(t.clientX-rect.left)*(canvas.width/rect.width); mouse.y=(t.clientY-rect.top)*(canvas.height/rect.height); }, {passive:false});

  startBtn.addEventListener('click', ()=>{ running = true; score = 0; obstacles.length = 0; lastSpawn = 0; restartBtn.style.display='none'; startBtn.style.display='none'; last = Date.now(); });
  restartBtn.addEventListener('click', ()=>{ running = true; score = 0; obstacles.length = 0; restartBtn.style.display='none'; startBtn.style.display='none'; last = Date.now(); });

  requestAnimationFrame(loop);
});
