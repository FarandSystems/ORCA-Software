#pragma once

class Tickable {
public:
    virtual ~Tickable() = default;
    virtual void onTick() = 0;
};