using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArms : MonoBehaviour
{
    enum BeamMovement
    {
        clockwise,
        anticlockwise,
        stop
    }

    private BeamMovement beamMovement;

    public float wait = 3f;
    public float spin_speed = 60f;
    float time;
    private bool is_going_left;

    void Start()
    {

    }


    void Update()
    {
        if (this.transform.position.x <= 0)
        {
            if (this.time >= 6)
            {
                this.time -= 6;

                switch (this.beamMovement)
                {
                    case BeamMovement.clockwise:
                        this.beamMovement = BeamMovement.stop;
                        break;
                    case BeamMovement.anticlockwise:
                        this.beamMovement = BeamMovement.stop;
                        break;
                    case BeamMovement.stop:
                        this.beamMovement = (BeamMovement)Random.Range(0, 1);
                        break;
                }

                this.is_going_left = this.is_going_left == false;
            }

            switch (this.beamMovement)
            {
                case BeamMovement.clockwise:
                    SpinAnticlockwise();
                    break;
                case BeamMovement.anticlockwise:
                    SpinClockwise();
                    break;
            }
        }

        this.time += Time.deltaTime;

    }



    void stop_spin()
    {

    }

    void SpinAnticlockwise()
    {
        this.transform.Rotate(0, 0, spin_speed * Time.deltaTime);
    }

    void SpinClockwise()
    {
        this.transform.Rotate(0, 0, spin_speed * -Time.deltaTime);
    }
}