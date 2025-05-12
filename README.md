# ClawWars

**2D PvP арена для двох гравців на одному ПК**  
Маг проти Воїна, фаєрболи проти мечів, щити проти вогню — і все це з піксельним шармом!

---

## 📖 Опис

У **ClawWars** два гравці змагаються на арені:
- У головному меню можна вибрати **Персонаж 1**, **Персонаж 2**, **Арену**, **Прив’язки клавіш** та керувати звуком (Mute).
- Після натискання **Start** завантажується бойова сцена, де:
  - **Маг**: присід, стрибок, рух вліво/вправо, коротка атака, довга атака (фаєрбол).
  - **Воїн**: присід, стрибок, рух вліво/вправо, коротка атака, довга атака (потужний удар), **щит**, що парирує.

---

## 🚀 Особливості

- **Дві ролі**: маг із дистанційними фаєрболами та воїн із ближнім боєм і щитом.  
- **Налаштування керування** на льоту у меню — перепризначай будь-яку клавішу.  
- **Динамічна музика**: рандомні треки в меню та на арені.  
- **Звукові ефекти**: кроки, удари, парирування, стрибки, падіння.  
- **КД-індикатори** в UI: іконки здібностей, таймери 0.00, вертикальний список під HP-баром.  
- **Нокбек і парирування**: відкидання при ударі та при атакуванні щита.  
- **Повна українська локалізація** та гумор у коді 😉

---

## 🔧 Встановлення та запуск

1. **Клонувати репозиторій:** ```git clone https://github.com/Volodia4/ClawWars.git```
2. **Відкрити проект** в Unity (версія **2021.3 LTS** або вище).
3. **Завантажити** сцену **MainMenu** та натиснути **Play**.

---

## 🎮 Управління (дефолтні клавіші)

| Дія                  | Гравець 1 | Гравець 2 |
| -------------------- | --------- | --------- |
| Вліво                |     A     | Keypad 4  |
| Вправо               |     D     | Keypad 6  |
| Стрибок              |     W     | Keypad 8  |
| Присід               |     S     | Keypad 2  |
| Коротка атака        |     F     | Keypad 7  |
| Довга атака          |     G     | Keypad 9  |
| Щит (воїн)           |     H     | Keypad 5  |
| -------------------- | --------------------- |
| Пауза / Налаштування |          Esc          |
| -------------------- | --------------------- |

*У меню → **Settings** можете будь-які перепризначити.*

---

## 🗂 Структура проєкту

```
Assets/
├── Animations/                                 # Папка з анімаціями
|   ├── Controllers/
|   |   ├── CatMagicianAnimator.controller
|   |   ├── CatWarriorAnimator.controller
|   |   └── FireballAnimator.controller
|   ├── CatMagicianCrouch.anim
|   ├── CatMagicianFall.anim
|   ├── CatMagicianIdle.anim
|   ├── CatMagicianJump.anim
|   ├── CatMagicianLongAttack.anim
|   ├── CatMagicianRunBack.anim
|   ├── CatMagicianRunFront.anim
|   ├── CatMagicianShortAttack.anim
|   ├── CatWarriorCrouch.anim
|   ├── CatWarriorFall.anim
|   ├── CatWarriorIdle.anim
|   ├── CatWarriorJump.anim
|   ├── CatWarriorLongAttack.anim
|   ├── CatWarriorRunBack.anim
|   ├── CatWarriorRunFront.anim
|   ├── CatWarriorShield.anim
|   ├── CatWarriorShortAttack.anim
|   ├── FireballDestroy.anim
|   ├── FireballFly.anim
|   └── FireballStart.anim
├── Prefab/                                     # Папка з префабами
│   ├── Arena_1.prefab
│   ├── Arena_2.prefab
│   ├── Arena_3.prefab
│   ├── Arena_4.prefab
│   ├── Arena_5.prefab
│   ├── Arena_6.prefab
│   ├── Arena_7.prefab
│   ├── Arena_8.prefab
│   ├── Arena_9.prefab
│   ├── Arena_10.prefab
│   ├── Arena_11.prefab
│   ├── Arena_12.prefab
│   ├── Arena_13.prefab
│   ├── Arena_14.prefab
│   ├── Arena_15.prefab
│   ├── CooldownSlot.prefab
│   ├── FireballPrefab.prefab
│   ├── MagicianPrefab.prefab
│   └── WarriorPrefab.prefab
├── Scenes/                                     # Папка зі сценами (головне меню та батлсцена)
│   ├── BattleScene.unity
│   └── MainMenu.unity
├── Scripts/                                    # Папка зі скриптами
│   ├── AudioManager.cs
│   ├── BattleSceneManager.cs
│   ├── CharacterAbilities.cs
│   ├── CooldownManager.cs
│   ├── CooldownSlot.cs
│   ├── DamageTrigger.cs
│   ├── Fireball.cs
│   ├── GameManager.cs
│   ├── HealthDefenseUpdater.cs
│   ├── MenuController.cs
│   ├── MuteButton.cs
│   ├── PauseWinMenu.cs
│   ├── Player.cs
│   ├── PlayerAttack.cs
│   ├── ScrollingBackground.cs
│   ├── Selector.cs
│   ├── MagicianAbilities.asset
│   └── WarriorAbilities.asset
├── Sounds/                                     # Папка зі звуками та музикою
│   ├── Music/
|   |   ├── battle1.ogg
|   |   ├── battle2.ogg
|   |   ├── battle3.ogg
|   |   ├── battle4.ogg
|   |   ├── battle5.ogg
|   |   ├── battle6.ogg
|   |   ├── track1.ogg
|   |   ├── track2.ogg
|   |   ├── track3.ogg
|   |   ├── track4.ogg
|   |   ├── track5.ogg
|   |   └── track6.ogg
│   └── SFX/
|   |   ├── Click.wav
|   |   ├── ExitGame.wav
|   |   ├── Fall.wav
|   |   ├── FireballExplode.wav
|   |   ├── FireballShoot.wav
|   |   ├── Jump.wav
|   |   ├── KeyChange.wav
|   |   ├── MageShort.wav
|   |   ├── Running.wav
|   |   ├── ShieldHit.wav
|   |   ├── StartGame.wav
|   |   ├── SwordLong.wav
|   |   ├── SwordShort.wav
|   |   ├── TakeDamage1.wav
|   |   ├── TakeDamage2.wav
|   |   ├── TakeDamage3.wav
|   |   ├── TakeDamage4.wav
|   |   ├── TakeDamage5.wav
|   |   ├── TakeDamage6.wav
|   |   └── Win.wav
├── Sprites/                                    # Папка зі всіма спрайтами та картинками
|   ├── BG/
|   |   ├── BackCityBG.png
|   |   ├── BG_1.jpg
|   |   ├── BG_2.jpg
|   |   ├── BG_3.jpg
|   |   ├── BG_4.jpg
|   |   ├── BG_5.jpg
|   |   ├── BG_6.jpg
|   |   ├── BG_7.jpg
|   |   ├── BG_8.jpg
|   |   ├── BG_9.jpg
|   |   ├── BG_10.jpg
|   |   ├── BG_11.jpg
|   |   ├── BG_12.jpg
|   |   ├── BG_13.jpg
|   |   ├── BG_14.jpg
|   |   ├── BG_15.jpg
|   |   ├── MainCityBG.png
|   |   ├── SettingsBG1.png
|   |   ├── SettingsBG2.png
|   |   ├── SkyGradientBG.png
|   |   └── SkyStarsBG.png
|   ├── Characters/
|   |   ├── CatMagician/
|   |   |   ├── CatMagicianBase.png
|   |   |   ├── CatMagicianHit.png
|   |   |   ├── CatMagicianHitLong.png
|   |   |   ├── CatMagicianJumpDown.png
|   |   |   ├── CatMagicianJumpUp.png
|   |   |   ├── CatMagicianRun.png
|   |   |   ├── CatMagicianSit.png
|   |   |   ├── CatMagicianStanding.png
|   |   |   ├── CatMagicianWalkBack.png
|   |   |   └── CatMagicianWalkFront.png
|   |   └── CatWarrior/
|   |   |   ├── CatWarriorBase.png
|   |   |   ├── CatWarriorHit.png
|   |   |   ├── CatWarriorHitLong.png
|   |   |   ├── CatWarriorJumpDown.png
|   |   |   ├── CatWarriorJumpUp.png
|   |   |   ├── CatWarriorRun.png
|   |   |   ├── CatWarriorShieldUse.png
|   |   |   ├── CatWarriorSit.png
|   |   |   ├── CatWarriorStanding.png
|   |   |   ├── CatWarriorWalkBack.png
|   |   |   └── CatWarriorWalkFront.png
|   └── Other/
|   |   ├── Fireball.png
|   |   └── Interface.png
└── TextMesh Pro/                               # Папка зі шрифтами
    └── ...
```

---

## 🤝 Як долучитися

1. **Fork** цього репозиторію.
2. Створи свою гілку:

   ```bash
   git checkout -b feature/назва-фічі
   ```
3. Зроби коміти та **Pull Request**.
4. Ми переглянемо, посміємося з твоїх жартів і замержимо в `dev`.

---

## 📝 Ліцензія

**MIT License** © 2025 Volodia4
Дозволено використовувати, копіювати та змінювати без обмежень, лише не забудь згадати авторів 😉

---

> **Підпис:**
> Володимир Іващишин Андійович
> Ілля Горобець Гелавич
> *ClawWars – бо життя занадто коротке без бою в пікселях*

```
```
